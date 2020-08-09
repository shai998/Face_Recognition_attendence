import numpy as np

#이미지 인식함수
#region 
def area_of(left_top, right_bottom):
    """
    두 모서리가 주어진 직사각형의 영역을 계산한다.
    Args:
        left_top(N, 2): 왼쪽 상단 모서리
        right_bottom(N, 2): 오른쪽 하단 모서리
    Returns:
        area(N): 해당 영역을 반환
==========================================================
    Compute the areas of rectangles given two corners.
    Args:
        left_top (N, 2): left top corner.
        right_bottom (N, 2): right bottom corner.
    Returns:
        area (N): return the area.
    """
    hw = np.clip(right_bottom - left_top, 0.0, None)
    return hw[..., 0] * hw[..., 1]

def iou_of(boxes0, boxes1, eps=1e-5):
    """
    상자의 교차 결합(Jaccard Index)을 반환하십시오.
    Args:
        box0(N, 4): 접지된 실제 상자.
        box1(N 또는 1, 4): 예측 상자.
        eps: 분모로 0을 피하기 위한 작은 숫자.
    Returns:
        iou(N): IoU 값.
===========================================================
    Return intersection-over-union (Jaccard index) of boxes.
    Args:
        boxes0 (N, 4): ground truth boxes.
        boxes1 (N or 1, 4): predicted boxes.
        eps: a small number to avoid 0 as denominator.
    Returns:
        iou (N): IoU values.
    """
    overlap_left_top = np.maximum(boxes0[..., :2], boxes1[..., :2])
    overlap_right_bottom = np.minimum(boxes0[..., 2:], boxes1[..., 2:])

    overlap_area = area_of(overlap_left_top, overlap_right_bottom)
    area0 = area_of(boxes0[..., :2], boxes0[..., 2:])
    area1 = area_of(boxes1[..., :2], boxes1[..., 2:])
    return overlap_area / (area0 + area1 - overlap_area + eps)

def hard_nms(box_scores, iou_threshold, top_k=-1, candidate_size=200):
    """
    최대값이 아닌 하드 압축을 수행하여 임계값보다 큰 상자를 필터링하십시오.
    Args:
        box_scores(N, 5): 모서리 형식 및 확률의 상자.
        iou_threshold: 유니언 임계값을 초과하는 교차점.
        top_k: top_k 결과 유지. k <= 0일 경우 모든 결과를 보관한다.
        candidate_size: 점수가 가장 높은 후보만 고려하십시오.
    Returns:
        picked: 보관된 상자의 색인 목록
========================================================================
    Perform hard non-maximum-supression to filter out boxes with iou greater
    than threshold
    Args:
        box_scores (N, 5): boxes in corner-form and probabilities.
        iou_threshold: intersection over union threshold.
        top_k: keep top_k results. If k <= 0, keep all the results.
        candidate_size: only consider the candidates with the highest scores.
    Returns:
        picked: a list of indexes of the kept boxes
    """
    scores = box_scores[:, -1]
    boxes = box_scores[:, :-1]
    picked = []
    indexes = np.argsort(scores)
    indexes = indexes[-candidate_size:]
    while len(indexes) > 0:
        current = indexes[-1]
        picked.append(current)
        if 0 < top_k == len(picked) or len(indexes) == 1:
            break
        current_box = boxes[current, :]
        indexes = indexes[:-1]
        rest_boxes = boxes[indexes, :]
        iou = iou_of(
            rest_boxes,
            np.expand_dims(current_box, axis=0),
        )
        indexes = indexes[iou <= iou_threshold]
    return box_scores[picked, :]

def predict(width, height, confidences, boxes, prob_threshold, iou_threshold=0.5, top_k=-1):
    """
    사람 얼굴이 들어 있는 상자 선택
    Args:
        width: 원본 이미지 너비
        height: 원본 영상 높이
        confidences(N, 2): 자신 배열
        boxes(N, 4): 모서리 형식의 상자 배열
        iou_threshold: 유니언 임계값을 초과하는 교차점.
        top_k: top_k 결과 유지. k <= 0일 경우 모든 결과를 보관한다.
    Returns:
        boxes(k, 4): 보관된 상자 배열
        labels(k): 보관된 각 상자에 대한 레이블 배열
        probs (k): 해당 라벨에 있는 각 상자에 대한 확률 배열
=====================================================================
    Select boxes that contain human faces
    Args:
        width: original image width
        height: original image height
        confidences (N, 2): confidence array
        boxes (N, 4): boxes array in corner-form
        iou_threshold: intersection over union threshold.
        top_k: keep top_k results. If k <= 0, keep all the results.
    Returns:
        boxes (k, 4): an array of boxes kept
        labels (k): an array of labels for each boxes kept
        probs (k): an array of probabilities for each boxes being in corresponding labels
    """
    boxes = boxes[0]
    confidences = confidences[0]
    picked_box_probs = []
    picked_labels = []
    for class_index in range(1, confidences.shape[1]):
        probs = confidences[:, class_index]
        mask = probs > prob_threshold
        probs = probs[mask]
        if probs.shape[0] == 0:
            continue
        subset_boxes = boxes[mask, :]
        box_probs = np.concatenate([subset_boxes, probs.reshape(-1, 1)], axis=1)
        box_probs = hard_nms(box_probs,
           iou_threshold=iou_threshold,
           top_k=top_k,
           )
        picked_box_probs.append(box_probs)
        picked_labels.extend([class_index] * box_probs.shape[0])
    if not picked_box_probs:
        return np.array([]), np.array([]), np.array([])
    picked_box_probs = np.concatenate(picked_box_probs)
    picked_box_probs[:, 0] *= width
    picked_box_probs[:, 1] *= height
    picked_box_probs[:, 2] *= width
    picked_box_probs[:, 3] *= height

    return picked_box_probs[:, :4].astype(np.int32), np.array(picked_labels), picked_box_probs[:, 4]
#endregion