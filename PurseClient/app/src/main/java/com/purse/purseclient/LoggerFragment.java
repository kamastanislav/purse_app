package com.purse.purseclient;


import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.CashAdapter;
import com.purse.entity.CategoryService;
import com.purse.entity.HistoryCash;
import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.helper.FilterData;
import com.purse.helper.MultiSpinner;
import com.purse.helper.WorkflowStatus;
import com.purse.services.RestService;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoggerFragment extends Fragment implements android.view.View.OnClickListener, AdapterView.OnItemClickListener {
    private View view;
    private ProgressDialog progress;
    private ListView loggerListView;

    private TextView fieldDateStartInterval;
    private TextView fieldDateEndInterval;

    private FilterData filterData;

    private MultiSpinner spinnerCategory;
    private MultiSpinner spinnerUser;

    private Calendar dateStartInterval;
    private Calendar dateEndInterval;

    public LoggerFragment() {
        filterData = new FilterData();
        dateStartInterval = getStartOfDay();
        dateEndInterval = getEndOfDay();
        filterData.Owner = new LinkedList<>();
        filterData.Owner.add(Constants.userCode);

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_logger, container, false);

        fieldDateStartInterval = (TextView) view.findViewById(R.id.interval_start_date_logger);
        fieldDateEndInterval = (TextView) view.findViewById(R.id.interval_end_date_logger);
        fieldDateStartInterval.setOnClickListener(this);
        fieldDateEndInterval.setOnClickListener(this);
        setInitialDateTime();

        TextView actionFilter = (TextView) view.findViewById(R.id.filter_logger_action);
        actionFilter.setOnClickListener(this);

        loggerListView = (ListView) view.findViewById(R.id.logger_list_view);
        progress = new ProgressDialog(view.getContext());

        Button btn = (Button) view.findViewById(R.id.send_filter_logger);
        btn.setOnClickListener(this);

        loadData();

        setFilterData();

        return view;
    }

    private void setFilterData() {

        spinnerCategory = (MultiSpinner) view.findViewById(R.id.filter_category_logger);
        Call<List<CategoryService>> callCategory = RestService.getService().getCategories();
        callCategory.enqueue(new Callback<List<CategoryService>>() {
            @Override
            public void onResponse(Call<List<CategoryService>> call, Response<List<CategoryService>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Integer> csCodes = new LinkedList<>();
                    List<String> csName = new LinkedList<>();
                    csCodes.add(0);
                    csName.add("Переводы");
                    for (CategoryService cs : response.body()) {
                        csCodes.add(cs.Code);
                        csName.add(cs.Name);
                    }

                    spinnerCategory.setItems(csName, csCodes);
                }
            }

            @Override
            public void onFailure(Call<List<CategoryService>> call, Throwable t) {

            }
        });

        spinnerUser = (MultiSpinner) view.findViewById(R.id.filter_user_logger);

        if (Constants.familyCode == Constants.DEFAULT_CODE) {
            spinnerUser.setVisibility(View.GONE);
        } else {
            Call<List<UserData>> callUsers = RestService.getService().usersList();

            callUsers.enqueue(new Callback<List<UserData>>() {
                @Override
                public void onResponse(Call<List<UserData>> call, Response<List<UserData>> response) {
                    if (response.isSuccessful() & response.body() != null) {
                        List<Integer> usersCodes = new LinkedList<>();
                        List<String> usersName = new LinkedList<>();
                        int index = 0;
                        List<UserData> users = response.body();
                        for (int i = 0; i < users.size(); i++) {
                            UserData user = users.get(i);
                            if (user.Code == Constants.familyCode)
                                index = i;
                            usersCodes.add(user.Code);
                            usersName.add(String.format("%1$s %2$s (%3$s)", user.FirstName, user.LastName, user.NickName));
                        }
                        spinnerUser.setItems(usersName, usersCodes, Constants.userName, index);
                    }
                }

                @Override
                public void onFailure(Call<List<UserData>> call, Throwable t) {

                }
            });
        }

        Spinner viewLogger = (Spinner) view.findViewById(R.id.filter_view_logger);

        List<String> listView = new ArrayList<String>();
        listView.add("Список");
        listView.add("Диаграмма");
        listView.add("Календарь");

        ArrayAdapter<String> dataListView = new ArrayAdapter<String>(view.getContext(),
                android.R.layout.simple_spinner_item, listView);
        dataListView.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        viewLogger.setAdapter(dataListView);

        viewLogger.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }

    private void loadData() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        filterData = getFilter();

        Call<List<HistoryCash>> call = RestService.getService().getHistoryUser(filterData);
        call.enqueue(new Callback<List<HistoryCash>>() {
            @Override
            public void onResponse(Call<List<HistoryCash>> call, Response<List<HistoryCash>> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    List<HistoryCash> historyCashes = response.body();

                    CashAdapter historyCashAdapter = new CashAdapter(view.getContext(), R.layout.view_cash_entity, historyCashes);

                    loggerListView.setAdapter(historyCashAdapter);

                } else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<List<HistoryCash>> call, Throwable t) {

            }
        });
    }

    private DatePickerDialog.OnDateSetListener dateEnd = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateEndInterval.set(Calendar.YEAR, year);
            dateEndInterval.set(Calendar.MONTH, monthOfYear);
            dateEndInterval.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    private void setInitialDateTime() {
        fieldDateStartInterval.setText(DateUtils.formatDateTime(view.getContext(),
                dateStartInterval.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
        fieldDateEndInterval.setText(DateUtils.formatDateTime(view.getContext(),
                dateEndInterval.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }


    private DatePickerDialog.OnDateSetListener dateStart = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateStartInterval.set(Calendar.YEAR, year);
            dateStartInterval.set(Calendar.MONTH, monthOfYear);
            dateStartInterval.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    private Calendar getStartOfDay() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DATE) - 30;
        calendar.set(year, month, day, 0, 0, 0);
        return calendar;
    }

    private Calendar getEndOfDay() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DATE);
        calendar.set(year, month, day, 23, 59, 59);
        return calendar;
    }

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.interval_start_date_logger)) {
            new DatePickerDialog(view.getContext(), dateStart,
                    dateStartInterval.get(Calendar.YEAR),
                    dateStartInterval.get(Calendar.MONTH),
                    dateStartInterval.get(Calendar.DAY_OF_MONTH))
                    .show();
        } else if (v == view.findViewById(R.id.interval_end_date_logger)) {
            new DatePickerDialog(view.getContext(), dateEnd,
                    dateEndInterval.get(Calendar.YEAR),
                    dateEndInterval.get(Calendar.MONTH),
                    dateEndInterval.get(Calendar.DAY_OF_MONTH))
                    .show();
        } else if (v == view.findViewById(R.id.filter_logger_action)) {
            LinearLayout layout = (LinearLayout) view.findViewById(R.id.logger_filter);
            layout.setVisibility(layout.getVisibility() == View.GONE ? View.VISIBLE : View.GONE);
        } else if (v == view.findViewById(R.id.send_filter_logger)) {
            loadData();
        }
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

    }

    public FilterData getFilter() {
        filterData.DateInterval = new LinkedList<>();
        filterData.DateInterval.add(dateStartInterval.getTimeInMillis());
        filterData.DateInterval.add(dateEndInterval.getTimeInMillis());

        if (spinnerCategory != null) {
            filterData.Category = spinnerCategory.getSelectedItems();
        }
        if (spinnerUser != null && spinnerUser.getVisibility() != View.GONE) {
            filterData.Owner = spinnerUser.getSelectedItems();
        }

        return filterData;
    }
}
