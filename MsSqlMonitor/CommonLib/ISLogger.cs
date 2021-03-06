﻿using System;

namespace CommonLib
{
    public interface ISLogger
    {
        void Debug(string msg);
        void Debug(string msg, Exception exception);

        void Info(string msg);
        void Info(string msg, Exception exception);

        void Error(string msg);
        void Error(string msg, Exception exception);
    }
}
