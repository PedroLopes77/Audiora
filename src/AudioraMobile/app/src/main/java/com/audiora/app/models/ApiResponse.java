package com.audiora.app.models;

public class ApiResponse<T> {
    private boolean success;
    private T data;
    private String errorMessage;
    private String errorCode;

    public boolean isSuccess()       { return success; }
    public T getData()               { return data; }
    public String getErrorMessage()  { return errorMessage; }
    public String getErrorCode()     { return errorCode; }
}