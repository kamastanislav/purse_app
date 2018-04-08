package com.purse.services;

import com.purse.entity.UserData;
import com.purse.helper.UserLogin;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface PurseService {
    //user controller

    @GET("user/list")
    public Call<List<UserData>> registration();

    @POST("user/registration")
    public Call<Integer> registrationUser(@Body UserData user);

    @POST("user/login")
    public Call<Integer> loginUser(@Body UserLogin userLogin);

    @POST("user/data/{code}")
    public Call<UserData> getUser(@Path("code") Integer code);

    @POST("user/logout/{code}")
    public Call<Boolean> logoutUser(@Path("code") Integer code);

    @PUT("/user/update/{code}")
    public void updateUser(@Path("code") Integer code, @Body UserData user);

    @PUT("/user/update_password/{code}")
    public void updatePasswordUser(@Path("code") Integer code, String password);

    @POST("/user/check_password/{code}")
    public void checkPasswordUser(@Path("code") Integer code, String password);

    @POST("/user/unique_field")
    public void uniqueField(Integer field, String value);
}


/*
* //i.e. http://localhost/api/institute/Students
    @GET("/institute/Students")
    public void getStudent(Callback<List<Student>> callback);

    //i.e. http://localhost/api/institute/Students/1
    //Get student record base on ID
    @GET("/institute/Students/{id}")
    public void getStudentById(@Path("id") Integer id,Callback<Student> callback);

    //i.e. http://localhost/api/institute/Students/1
    //Delete student record base on ID
    @DELETE("/institute/Students/{id}")
    public void deleteStudentById(@Path("id") Integer id,Callback<Student> callback);

    //i.e. http://localhost/api/institute/Students/1
    //PUT student record and post content in HTTP request BODY
    @PUT("/institute/Students/{id}")
    public void updateStudentById(@Path("id") Integer id,@Body Student student,Callback<Student> callback);

    //i.e. http://localhost/api/institute/Students
    //Add student record and post content in HTTP request BODY
    @POST("/institute/Students")
    public void addStudent(@Body Student student,Callback<Student> callback);
* */