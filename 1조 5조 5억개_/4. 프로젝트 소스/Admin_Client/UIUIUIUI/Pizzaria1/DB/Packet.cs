using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzaria1
{
    class Packet
    {
        #region 싱글톤 패턴
        public static Packet Instance { get; private set; }

        static Packet()
        {
            Instance = new Packet();
        }

        private Packet()
        {

        }
        #endregion

        //현재인원, 총인원
        public string SendIC()
        {
            string Query = "SELECT (SELECT COUNT(STU_ID) FROM ATT_TEST.DBO.STUDENT), (SELECT COUNT(DISTINCT STU_ID)" +
                " FROM INOUT WHERE INOUT_BOOL = 1 AND INOUT_ID = ANY(SELECT MAX(I.INOUT_ID) AS MAX_ID" +
                " FROM STUDENT S, INOUT I WHERE S.STU_ID = I.STU_ID GROUP BY S.STU_NAME))";

            string msg = null;
            msg += "COUNT_SI@";         // 회원 가입 요청 메시지
            msg += Query;

            return msg;
        }

        //출,입 횟수
        public string SendIO()
        {
            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            string Query = "SELECT (SELECT COUNT(IN_TIME) FROM INOUT WHERE INOUT_DATE = '" + nowdate + "'),(SELECT COUNT(OUT_TIME) FROM INOUT WHERE INOUT_DATE = '" + nowdate + "')";

            string msg = null;
            msg += "COUNT_IO@";
            msg += Query;

            return msg;
        }

        //당일 지각자 수
        public string SendNLA()
        {
            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            string Query = "SELECT COUNT(ATT_LATE) FROM DAILY_ATTEND WHERE ATT_LATE = 1 AND ATT_DATE = '" + nowdate + "'";

            string msg = null;
            msg += "COUNT_CL@";
            msg += Query;

            return msg;
        }

        //이름 현재위치
        public string SendNB()
        {
            string Query = "SELECT S.STU_NAME, I.INOUT_BOOL FROM STUDENT S, INOUT I WHERE S.STU_ID = I.STU_ID AND I.INOUT_ID = ANY(SELECT MAX(I.INOUT_ID) AS MAX_ID"+
                            " FROM STUDENT S, INOUT I WHERE S.STU_ID = I.STU_ID GROUP BY S.STU_NAME)";

            string msg = null;
            msg += "SELECT_NB@";         // 회원 가입 요청 메시지
            msg += Query;

            return msg;
        }

        //이름 학과 학번 출석 지각
        public string SendAll()
        {
            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            string Query = "SELECT S.STU_ID, S.STU_NAME, S.STU_DEP, D.ATT_BOOL, D.ATT_LATE FROM STUDENT S, DAILY_ATTEND D WHERE S.STU_ID = D.STU_ID  AND ATT_DATE = '" + nowdate + "'";

            string msg = null;
            msg += "SELECT_SD@";
            msg += Query;

            return msg;
        }

        public string SendName()
        {
            string Query = "SELECT S.STU_NAME, I.INOUT_BOOL, I.IN_TIME, I.OUT_TIME, I.INOUT_DATE FROM STUDENT S, INOUT I WHERE S.STU_ID = I.STU_ID";

            string msg = null;
            msg += "SELECT_NBTT@";
            msg += Query;

            return msg;
        }

        public string Senddate(string date)
        {
            string Query = "SELECT S.STU_NAME, D.ATT_BOOL, D.ATT_LATE FROM STUDENT S, DAILY_ATTEND D WHERE S.STU_ID = D.STU_ID AND d.att_date = '" + date+ "'";

            string msg = null;
            msg += "SELECT_SA@";
            msg += Query;

            return msg;
        }

        public string SendChat()
        {
            string Query = "SELECT STU_NAME FROM STUDENT";

            string msg = null;
            msg += "SELECT_N@";
            msg += Query;

            return msg;
        }

        public string SendName(string name)
        {
            //string Query = "SELECT S.STU_NAME, D.ATT_BOOL, D.ATT_LATE FROM STUDENT S, DAILY_ATTEND D WHERE S.STU_ID = D.STU_ID AND d.att_date = '" + date + "'";
            string Query = "SELECT COUNT(D.ATT_LATE) FROM STUDENT S, DAILY_ATTEND D WHERE S.STU_ID = D.STU_ID AND S.STU_NAME = '" + name + "' AND D.ATT_LATE = 1";

            string msg = null;
            msg += "COUNT_ML@";
            msg += Query;

            return msg;
        }

        public string SendTime(string name)
        {
            string Query = "SELECT ATT_TIME FROM DAILY_ATTEND D, STUDENT S WHERE D.STU_ID = S.STU_ID AND S.STU_NAME = '" + name + "'";

            string msg = null;
            msg += "SELECT_MT@";
            msg += Query;

            return msg;
        }
    }
}
