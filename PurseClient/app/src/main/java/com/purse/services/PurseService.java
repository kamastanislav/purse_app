package com.purse.services;

import com.purse.entity.CategoryService;
import com.purse.entity.Flight;
import com.purse.entity.HistoryCash;
import com.purse.entity.Information;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.helper.FilterData;
import com.purse.helper.FilterPlan;
import com.purse.helper.UserLogin;

import java.math.BigDecimal;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
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

    @POST("user/check_password")
    public Call<Boolean> checkPassword(@Query("password") String password);

    @PUT("user/update")
    public Call<UserData> updateUser(@Body UserData user);

    @PUT("user/update_password")
    public Call<Boolean> updatePasswordUser(@Query("password") String password);

    /*family controller*/
    @POST("family/info")
    public Call<List<Boolean>> infoFamily();

    @POST("family/users")
    public Call<List<UserData>> usersList();

    @POST("family/add_user")
    public Call<Boolean> addUserInFamily(@Query("code") Integer code);

    @POST("family/search_users")
    public Call<List<UserData>> searchUsersList(@Query("name") String name);

    @POST("family/create_family")
    public Call<Integer> createFamily();

    /*plan controller*/
    @POST("plan/filter_data")
    public Call<FilterPlan> setFilterPlan();

    @POST("plan/categories")
    public Call<List<CategoryService>> getCategories();

    @POST("plan/create")
    public Call<Boolean> savePlan(@Body Plan plan);

    @POST("plan/update/{code}")
    public Call<Boolean> updatePlan(@Path("code") Integer code, @Body Plan plan);

    @POST("plan/list")
    public Call<List<Plan>> listPlan(@Body FilterData filter);

    @POST("plan/delete/{code}")
    public Call<Boolean> deletePlan(@Path("code") Integer code);

    @POST("plan/data/{code}")
    public Call<Plan> plan(@Path("code") Integer code);

    @POST("plan/approve/{code}")
    public Call<Boolean> approvePlan(@Path("code") Integer code);

    @POST("plan/undelete/{code}")
    public Call<Boolean> undeletePlan(@Path("code") Integer code);

    /*flight controller*/
    @POST("flight/create_flight")
    public Call<Boolean> createFlight(@Query("planCode") Integer planCode,
                                      @Query("plannedBudget") BigDecimal plannedBudget,
                                      @Query("comment") String comment);

    @POST("flight/plan/{code}")
    public Call<List<Flight>> flightsPlan(@Path("code") Integer code);

    @POST("flight/data/{code}")
    public Call<Flight> flightPlan(@Path("code") Integer code);

    @PUT("flight/approve/{code}")
    public Call<Boolean> approveFlightPlan(@Path("code") Integer code, @Query("actualBudget") BigDecimal actualBudget);

    @DELETE("flight/delete/{code}")
    public Call<Boolean> deleteFlightPlan(@Path("code") Integer code);

    @PUT("flight/update_flight/{code}")
    public Call<Boolean> updateFlightPlan(@Path("code") Integer code,
                                          @Query("plannedBudget") BigDecimal plannedBudget,
                                          @Query("comment") String comment);

    /*history controller*/
    @POST("history/budget_replenishment")
    public Call<Boolean> budgetReplenishment(@Query("budget") BigDecimal budget);

    @POST("history/budget_replenishment_other_user/{code}")
    public Call<Boolean> budgetReplenishmentOtherUser(@Path("code") Integer code, @Query("budget") BigDecimal budget);

    @POST("history/user_cash")
    public Call<List<HistoryCash>> getHistoryUser(@Body FilterData filter);

    @POST("history/info")
    public Call<List<Information>> getInfoHistory();
}