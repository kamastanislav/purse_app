package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.purse.array_adapter.HistoryAdapter;
import com.purse.entity.Information;
import com.purse.services.RestService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class HistoryFragment extends Fragment {
    private View view;

    private ProgressDialog progress;
    private ListView historyListView;

    public HistoryFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        view = inflater.inflate(R.layout.fragment_history, container, false);
        historyListView = (ListView)view.findViewById(R.id.history_list_view);
        progress = new ProgressDialog(view.getContext());
        loadData();

        return view;
    }

    private void loadData() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<Information>> call = RestService.getService().getInfoHistory();
        call.enqueue(new Callback<List<Information>>() {
            @Override
            public void onResponse(Call<List<Information>> call, Response<List<Information>> response) {
                progress.dismiss();
                if(response.isSuccessful()) {
                    List<Information> information = response.body();

                    HistoryAdapter historyAdapter = new HistoryAdapter(view.getContext(), R.layout.view_history_entity, information);

                    historyListView.setAdapter(historyAdapter);

                }
                else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<List<Information>> call, Throwable t) {

            }
        });
    }

}
