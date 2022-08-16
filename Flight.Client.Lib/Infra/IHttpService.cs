using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Client.Lib.Infra
{
    public interface IHttpService
    {
        Task<R> PostAsync<T, R>(string url, T payload);

    }
}
