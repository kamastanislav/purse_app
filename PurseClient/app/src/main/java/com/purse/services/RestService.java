package com.purse.services;

public class RestService {
    private static final String URL = "http://10.0.2.2/api/";
    private retrofit.RestAdapter restAdapter;
    private PurseService apiService;

    public RestService()
    {
        restAdapter = new retrofit.RestAdapter.Builder()
                .setEndpoint(URL)
                .setLogLevel(retrofit.RestAdapter.LogLevel.FULL)
                .build();

        apiService = restAdapter.create(PurseService.class);
    }

    public PurseService getService()
    {
        return apiService;
    }
}
