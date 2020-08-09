def Parse_Data(result):
    split_result = result.split('@')
    
    if split_result[0] == 'SELECT_ID_ACK':
        return Parse_ID_Data(split_result[1])
    elif split_result[0] == 'SELECT_BD_ACK':
        return Parse_BD_Data(split_result[1])
    elif split_result[0] == 'COUNT_STU_ACK':
        return split_result[1]
    elif split_result[0] == 'SELECT_S_ACK':
        return Parse_N_Data(split_result[1])
    elif split_result[0] == 'DB_CHANGED':
        pass
        
def Parse_N_Data(result):
    Split_DATA_1 = []
    Split_DATA_2 = []
    STU_NAME = []

    Split_DATA_1 = result.split('/')
    for i in Split_DATA_1:
        Split_DATA_2 = i.split('#')
        STU_NAME.append(Split_DATA_2[1])

    return STU_NAME

def Parse_ID_Data(result):
    STU_ID = []
    STU_ID = result.split('/')
       
    return STU_ID

def Parse_BD_Data(result):
    Split_DATA_1 = []
    Split_DATA_2 = []

    Split_DATA_1 = result.split('/')
    for i in Split_DATA_1:
        Split_DATA_2 = i.split('#')
        ATT_BOOL = (Split_DATA_2[0])
        ATT_DATE = (Split_DATA_2[1])

    return ATT_BOOL, ATT_DATE