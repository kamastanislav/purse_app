package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Intent;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.purse.helper.Constants;
import com.purse.helper.UserLogin;
import com.purse.services.PurseService;
import com.purse.services.RestService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;


public class LoginActivity extends AppCompatActivity {
    private EditText fieldNick;
    private EditText fieldPassword;
 //   private PurseService apiService = Constants.getRestService();
   // private RestService restAdapter;
    private ProgressDialog progress;
    private Intent intent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        fieldNick = (EditText) findViewById(R.id.nickName);
        fieldPassword = (EditText) findViewById(R.id.password);
      //  restAdapter = new RestService();
        progress = new ProgressDialog(this);
        intent = new Intent(this, UserActivity.class);
    }

    public  void  login(View view) {
        String login = fieldNick.getText().toString();
        String password = fieldPassword.getText().toString();

        boolean hasEmptyField = false;

        if (login.isEmpty()) {
            fieldNick.setError("Nick is required!");
            hasEmptyField = true;
        }

        if (password.isEmpty()) {
            fieldNick.setError("Password is required!");
            hasEmptyField = true;
        }
        if (hasEmptyField)
            return;

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false); // disable dismiss by tapping outside of the dialog
        progress.show();

        UserLogin userLogin = new UserLogin();
        userLogin.NickName = login;
        userLogin.Password = password;

        Call<Integer> call = RestService.getService().loginUser(userLogin);
        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Integer code = response.body();


                    if (code != null && code != Constants.DEFAULT_CODE) {
                        intent.putExtra(Constants.USER_CODE, code);
                        startActivity(intent);
                    } else {
                        Toast.makeText(LoginActivity.this, "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(LoginActivity.this, "NO", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {

            }
        });

    }

    public void registrationView(View view) {
        Intent intent = new Intent(this, RegistrationActivity.class);

        startActivity(intent);
    }
}

