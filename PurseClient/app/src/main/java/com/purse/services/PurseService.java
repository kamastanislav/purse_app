package com.purse.services;

import com.purse.entity.Flight;
import com.purse.entity.HistoryCash;
import com.purse.entity.Information;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.helper.FilterPlan;
import com.purse.helper.UserLogin;

import java.math.BigDecimal;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface PurseService {
    /*user controller*/
    @GET("user/list")
    public Call<List<UserData>> registration();

    @POST("user/registration")
    public Call<Integer> registrationUser(@Body UserData user);

    @POST("user/login")
    public Call<Integer> loginUser(@Body UserLogin userLogin);

    @POST("user/data/{code}")
    public Call<UserData> getUser(@Path("code") Integer code);

    @POST("user/session")
    public Call<UserData> getSessionUser();

    @POST("user/logout")
    public Call<Boolean> logoutUser();

    @POST("user/unique_field")
    public Call<Boolean> uniqueField(@Query("field") Integer field, @Query("value") String value);

    @POST("user/name")
    public Call<String> getNameUser();

    /*family controller*/
    @POST("family/having_family")
    public Call<Boolean> havingFamily();

    @POST("family/users")
    public Call<List<UserData>> usersList();

    /*plan controller*/
    @POST("plan/filter_data")
    public Call<FilterPlan> setFilterPlan();

    @POST("plan/create")
    public Call<Boolean> savePlan(@Body Plan plan);

    @POST("plan/owner/{code}")
    public Call<List<Plan>> allPlanOwner(@Path("code") Integer code);

    @POST("plan/data/{code}")
    public Call<Plan> plan(@Path("code") Integer code);

    /*flight controller*/
    @POST("flight/create")
    public Call<Boolean> createFlight(@Body Flight flight);

    @POST("flight/plan/{code}")
    public Call<List<Flight>> flightsPlan(@Path("code") Integer code);

    @POST("flight/data/{code}")
    public Call<Flight> flightPlan(@Path("code") Integer code);

    /*history controller*/
    @POST("history/budget_replenishment")
    public Call<Boolean> budgetReplenishment(@Query("budget") BigDecimal budget);

    @POST("history/budget_replenishment_other_user/{code}")
    public Call<Boolean> budgetReplenishmentOtherUser(@Path("code") Integer code, @Query("budget") BigDecimal budget);

    @POST("history/user_cash/{code}")
    public Call<List<HistoryCash>> getHistoryUser(@Path("code") Integer code);

    @POST("history/info")
    public Call<List<Information>> getInfoHistory();
}