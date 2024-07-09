﻿namespace FC.Codeflix.Catalog.Api.ApiModels.Response;

public class ApiResponse<TData> where TData : class
{
    public TData Data { get; private set; }

    public ApiResponse(TData data)
        => Data = data;
}
