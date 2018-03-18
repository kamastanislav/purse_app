package com.purse.services;

import com.purse.entity.UserData;
import java.util.List;

import retrofit.Callback;
import retrofit.http.Body;
import retrofit.http.DELETE;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.PUT;
import retrofit.http.Path;

public interface PurseService {

    @GET("/")
    public void get(Callback<List<UserData>> callback);
}
