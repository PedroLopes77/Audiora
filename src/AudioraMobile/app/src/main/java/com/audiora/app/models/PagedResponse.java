package com.audiora.app.models;

import java.util.List;

public class PagedResponse<T> {
    private List<T> data;
    private int page;
    private int pageSize;
    private int totalCount;
    private int totalPages;
    private boolean hasNext;
    private boolean hasPrevious;

    public List<T> getData()    { return data; }
    public int getTotalCount()  { return totalCount; }
    public int getTotalPages()  { return totalPages; }
    public boolean isHasNext()  { return hasNext; }
}