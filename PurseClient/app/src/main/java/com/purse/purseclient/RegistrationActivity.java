package com.purse.purseclient;

import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.TextUtils;
import android.text.format.DateUtils;
import android.view.View;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import java.util.Calendar;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegistrationActivity extends AppCompatActivity {

    private TextView fieldDateBirthday;
    private Calendar dateAndTime = Calendar.getInstance();
    private EditText fieldNick;
    private EditText fieldPassword;
    private EditText fieldEmail;
    private EditText fieldConfirmPassword;
    private EditText fieldFirstName;
    private EditText fieldLastName;
    private EditText fieldPhone;
   // private RestService restAdapter;
    private ProgressDialog progress;
    private Intent intent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_registration);

        fieldNick = (EditText)findViewById(R.id.nickName);
        fieldPassword = (EditText)findViewById(R.id.password);
        fieldEmail = (EditText)findViewById(R.id.email);
        fieldConfirmPassword = (EditText)findViewById(R.id.confirmPassword);
        fieldFirstName = (EditText)findViewById(R.id.firstName);
        fieldLastName = (EditText)findViewById(R.id.lastName);
        fieldPhone = (EditText)findViewById(R.id.phone);

        fieldDateBirthday = (TextView)findViewById(R.id.date_birthday);
        setInitialDateTime();
        progress = new ProgressDialog(this);
    //    restAdapter = new RestService();
        intent = new Intent(this, UserActivity.class);
    }

    private void setInitialDateTime() {

        fieldDateBirthday.setText(DateUtils.formatDateTime(this,
                dateAndTime.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }

    private DatePickerDialog.OnDateSetListener d=new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateAndTime.set(Calendar.YEAR, year);
            dateAndTime.set(Calendar.MONTH, monthOfYear);
            dateAndTime.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    public void setDate(View v) {
        new DatePickerDialog(RegistrationActivity.this, d,
                dateAndTime.get(Calendar.YEAR),
                dateAndTime.get(Calendar.MONTH),
                dateAndTime.get(Calendar.DAY_OF_MONTH))
                .show();
    }

    public void registrationUser(View view) {
        String nickName = fieldNick.getText().toString();
        String email = fieldEmail.getText().toString();
        String password = fieldPassword.getText().toString();
        String confirmPassword = fieldConfirmPassword.getText().toString();
        String firstName = fieldFirstName.getText().toString();
        String lastName = fieldLastName.getText().toString();
        String phone = fieldPhone.getText().toString();

        boolean hasEmptyField = false;

        if (TextUtils.isEmpty(nickName)) {
            fieldNick.setError("Nick name is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(email)) {
            fieldEmail.setError("Email is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(password)) {
            fieldPassword.setError("Password is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(confirmPassword)){
            fieldConfirmPassword.setError("Confirm Password is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(firstName)) {
            fieldFirstName.setError("First name is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(lastName)) {
            fieldLastName.setError("Last name is required!");
            hasEmptyField = true;
        }

        if (TextUtils.isEmpty(phone)) {
            fieldPhone.setError("Phone is required!");
            hasEmptyField = true;
        }

        if (hasEmptyField)
            return;

        UserData userData = new UserData();

        userData.NickName = nickName;
        userData.Email = email;
        userData.FirstName = firstName;
        userData.LastName = lastName;
        userData.Phone = phone;
        userData.Birthday = dateAndTime.getTimeInMillis();
        userData.Password = password;

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Integer> call = RestService.getService().registrationUser(userData);
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
                        Toast.makeText(RegistrationActivity.this, "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(RegistrationActivity.this, "NO", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {

            }
        });
     /*   restService.getService().registrationUser(userData, new Callback<Integer>(){
            @Override
            public void success(Integer code, Response response) {
                progress.dismiss();
                if (code != Constants.DEFAULT_CODE) {
                    intent.putExtra(Constants.USER_CODE, code);
                    startActivity(intent);
                }
                else {
                    Toast.makeText(RegistrationActivity.this, "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void failure(RetrofitError error) {
                progress.dismiss();
                Toast.makeText(RegistrationActivity.this, error.getMessage().toString(), Toast.LENGTH_LONG).show();

            }
        });*/
    }
}
