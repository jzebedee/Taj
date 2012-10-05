namespace Taj.Messages
{
    public static class MessageTypes
    {
        public const int
            MSG_ALTLOGONREPLY = 0x72657032,
            //MNEM: 'rep2', server → client
            MSG_ASSETNEW = 0x61417374,
            //MNEM: 'aAst', server - client
            MSG_ASSETQUERY = 0x71417374,
            //MNEM: 'qAst', server ↔ client
            MSG_ASSETREGI = 0x72417374,
            //MNEM: 'rAst', server ← client
            MSG_ASSETSEND = 0x73417374,
            //MNEM: 'sAst', server → client
            MSG_AUTHENTICATE = 0x61757468,
            //MNEM: 'auth', server → client
            MSG_AUTHRESPONSE = 0x61757472,
            //MNEM: 'autr', server ← client
            MSG_BLOWTHRU = 0x626c6f77,
            //MNEM: 'blow', server ↔ client
            MSG_DISPLAYURL = 0x6475726c,
            //MNEM: 'durl', server → client
            MSG_DIYIT = 0x72796974,
            //MNEM: 'ryit', server - client
            MSG_DOORLOCK = 0x6c6f636b,
            //MNEM: 'lock', server ↔ client
            MSG_DOORUNLOCK = 0x756e6c6f,
            //MNEM: 'unlo', server ↔ client
            MSG_DRAW = 0x64726177,
            //MNEM: 'draw', server ↔ client
            MSG_EXTENDEDINFO = 0x73496e66,
            //MNEM: 'sInf', server ↔ client
            MSG_FILENOTFND = 0x666e6665,
            //MNEM: 'fnfe', server → client
            MSG_FILEQUERY = 0x7146696c,
            //MNEM: 'qFil', server ← client
            MSG_FILESEND = 0x7346696c,
            //MNEM: 'sFil', server → client
            MSG_GMSG = 0x676d7367,
            //MNEM: 'gmsg', server ← client
            MSG_HTTPSERVER = 0x48545450,
            //MNEM: 'HTTP', server → client
            MSG_INITCONNECTION = 0x634c6f67,
            //MNEM: 'cLog', server - client
            MSG_KILLUSER = 0x6b696c6c,
            //MNEM: 'kill', server ← client
            MSG_LISTOFALLROOMS = 0x724c7374,
            //MNEM: 'rLst', server ↔ client
            MSG_LISTOFALLUSERS = 0x754c7374,
            //MNEM: 'uLst', server ↔ client
            MSG_LOGOFF = 0x62796520,
            //MNEM: 'bye ', server ↔ client
            MSG_LOGON = 0x72656769,
            //MNEM: 'regi', server ← client
            MSG_NAVERROR = 0x73457272,
            //MNEM: 'sErr', server → client
            MSG_NOOP = 0x4e4f4f50,
            //MNEM: 'NOOP', server - client
            MSG_PICTDEL = 0x46505371,
            //MNEM: 'FPSq', server - client
            MSG_PICTMOVE = 0x704c6f63,
            //MNEM: 'pLoc', server ↔ client
            MSG_PICTNEW = 0x6e506374,
            //MNEM: 'nPct', server - client
            MSG_PICTSETDESC = 0x73506374,
            //MNEM: 'sPct', server - client
            MSG_PING = 0x70696e67,
            //MNEM: 'ping', server ↔ client
            MSG_PONG = 0x706f6e67,
            //MNEM: 'pong', server ↔ client
            MSG_PROPDEL = 0x64507270,
            //MNEM: 'dPrp', server ↔ client
            MSG_PROPMOVE = 0x6d507270,
            //MNEM: 'mPrp', server ↔ client
            MSG_PROPNEW = 0x6e507270,
            //MNEM: 'nPrp', server ↔ client
            MSG_PROPSETDESC = 0x73507270,
            //MNEM: 'sPrp', server - client
            MSG_RESPORT = 0x72657370,
            //MNEM: 'resp', server - client
            MSG_RMSG = 0x726d7367,
            //MNEM: 'rmsg', server ← client
            MSG_ROOMDESC = 0x726f6f6d,
            //MNEM: 'room', server → client
            MSG_ROOMDESCEND = 0x656e6472,
            //MNEM: 'endr', server → client
            MSG_ROOMGOTO = 0x6e617652,
            //MNEM: 'navR', server ← client
            MSG_ROOMNEW = 0x6e526f6d,
            //MNEM: 'nRom', server ← client
            MSG_ROOMSETDESC = 0x73526f6d,
            //MNEM: 'sRom', server ↔ client
            MSG_SERVERDOWN = 0x646f776e,
            //MNEM: 'down', server → client
            MSG_SERVERINFO = 0x73696e66,
            //MNEM: 'sinf', server → client
            MSG_SERVERUP = 0x696e6974,
            //MNEM: 'init', server - client
            MSG_SMSG = 0x736d7367,
            //MNEM: 'smsg', server ← client
            MSG_SPOTDEL = 0x6f705364,
            //MNEM: 'opSd', server ← client
            MSG_SPOTMOVE = 0x636f4c73,
            //MNEM: 'coLs', server ↔ client
            MSG_SPOTNEW = 0x6f70536e,
            //MNEM: 'opSn', server ← client
            MSG_SPOTSETDESC = 0x6f705373,
            //MNEM: 'opSs', server - client
            MSG_SPOTSTATE = 0x73537461,
            //MNEM: 'sSta', server ↔ client
            MSG_SUPERUSER = 0x73757372,
            //MNEM: 'susr', server ← client
            MSG_TALK = 0x74616c6b,
            //MNEM: 'talk', server ↔ client
            MSG_TIMYID = 0x74696d79,
            //MNEM: 'timy', server - client
            MSG_TIYID = 0x74697972,
            //MNEM: 'tiyr', server ↔ client
            MSG_TROPSER = 0x70736572,
            //MNEM: 'pser', server - client
            MSG_USERCOLOR = 0x75737243,
            //MNEM: 'usrC', server ↔ client
            MSG_USERDESC = 0x75737244,
            //MNEM: 'usrD', server ↔ client
            MSG_USERENTER = 0x77707273,
            //MNEM: 'wprs', server - client
            MSG_USEREXIT = 0x65707273,
            //MNEM: 'eprs', server → client
            MSG_USERFACE = 0x75737246,
            //MNEM: 'usrF', server ↔ client
            MSG_USERLIST = 0x72707273,
            //MNEM: 'rprs', server → client
            MSG_USERLOG = 0x6c6f6720,
            //MNEM: 'log ', server → client
            MSG_USERMOVE = 0x754c6f63,
            //MNEM: 'uLoc', server ↔ client
            MSG_USERNAME = 0x7573724e,
            //MNEM: 'usrN', server ↔ client
            MSG_USERNEW = 0x6e707273,
            //MNEM: 'nprs', server → client
            MSG_USERPROP = 0x75737250,
            //MNEM: 'usrP', server ↔ client
            MSG_USERSTATUS = 0x75537461,
            //MNEM: 'uSta', server → client
            MSG_VERSION = 0x76657273,
            //MNEM: 'vers', server → client
            MSG_WHISPER = 0x77686973,
            //MNEM: 'whis', server ↔ client
            MSG_WMSG = 0x776d7367,
            //MNEM: 'wmsg', server - client
            MSG_XTALK = 0x78746c6b,
            //MNEM: 'xtlk', server ↔ client
            MSG_XWHISPER = 0x78776973 //MNEM: 'xwis', server ↔ client
            ;
    }
}