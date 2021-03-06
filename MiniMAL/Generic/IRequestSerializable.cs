﻿namespace MiniMAL.Generic
{
    internal interface IRequestSerializable<in TRequestData>
        where TRequestData : IRequestData
    {
        void GetData(TRequestData data);
        string SerializeDataToString();
    }
}