package com.purse.services;

import com.purse.entity.UserData;
import java.util.List;

import retrofit.Callback;
import retrofit.http.Body;
import retrofit.http.DELETE;
import retrofit.http.Field;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.PUT;
import retrofit.http.Path;

public interface PurseService {
    //user controller

    @GET("/user/list")
    public void registration(Callback<List<UserData>> callback);

    @POST("/user/registration")
    public void registrationUser(@Body UserData user, String password);

    @POST("/user/login")
    public void loginUser(@Field("login") String login, @Field("password") String password, Callback<UserData> callback);

    @POST("/user/data/{id}")
    public void getUser(@Path("id") Integer id);

    @POST("/user/logout/{id}")
    public void logoutUser(@Path("id") Integer id);

    @PUT("/user/update/{id}")
    public void updateUser(@Path("id") Integer id, @Body UserData user);

    @PUT("/user/update_password/{id}")
    public void updatePasswordUser(@Path("id") Integer id, String password);

    @POST("/user/check_password/{id}")
    public void checkPasswordUser(@Path("id") Integer id, String password);

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