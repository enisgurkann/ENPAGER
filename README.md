
# ENPAGER - EFCORE Pager Tool
<img src="https://github.com/enisgurkann/ENPAGER/blob/master/ENPAGER.png?raw=true" data-canonical-src="https://github.com/enisgurkann/ENPAGER/blob/master/ENPAGER.png?raw=true" width="150" height="150" />


[![GitHub](https://img.shields.io/github/license/enisgurkann/ENPAGER?color=594ae2&logo=github&style=flat-square)](https://github.com/enisgurkann/ENPAGER/blob/master/LICENSE)
[![GitHub Repo stars](https://img.shields.io/github/stars/enisgurkann/ENPAGER?color=594ae2&style=flat-square&logo=github)](https://github.com/enisgurkann/ENPAGER/stargazers)
[![GitHub last commit](https://img.shields.io/github/last-commit/enisgurkann/ENPAGER?color=594ae2&style=flat-square&logo=github)](https://github.com/mudblazor/mudblazor)
[![Contributors](https://img.shields.io/github/contributors/enisgurkann/ENPAGER?color=594ae2&style=flat-square&logo=github)](https://github.com/enisgurkann/ENPAGER/graphs/contributors)
[![Discussions](https://img.shields.io/github/discussions/enisgurkann/ENPAGER?color=594ae2&logo=github&style=flat-square)](https://github.com/enisgurkann/ENPAGER/discussions)
[![Nuget version](https://img.shields.io/nuget/v/ENPAGER?color=ff4081&label=nuget%20version&logo=nuget&style=flat-square)](https://www.nuget.org/packages/ENPAGER/)
[![Nuget downloads](https://img.shields.io/nuget/dt/ENPAGER?color=ff4081&label=nuget%20downloads&logo=nuget&style=flat-square)](https://www.nuget.org/packages/ENPAGER/)



It is a plugin that allows us to quickly paginate while using Entity framework.
 
 ## Methods
  ToPagedListAsync,ToPagedList

## Entity Framework Core Pager Provider Usage

```
PM> Install-Package ENPAGER
```


```
PM> Standart Paging
```

```csharp


        var totalCount = await await _context.Where(x => x.Name.Contains('Enis')).CountAsync();
        var items = await await _context
                          .Customers
                          .Where(x => x.Name.Contains('Enis'))
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();
        int totalPages = (int) Math.Ceiling((double) TotalCount / PageSize);
        bool hasPreviousPage = pageIndex > 1;
        bool hasNextPage = pageIndex < totalPages;
        var customersPagedList = new {
                                      PageIndex = pageIndex,
                                      PageSize = pageSize,
                                      TotalCount = totalCount,
                                      TotalPages = totalPages,
                                      HasPreviousPage = hasPreviousPage,
                                      HasNextPage = hasNextPage
                                     };
```
 
```
PM> Using ToPagedListAsync
```

```csharp

        var customersPagedList = await _context
        .Customers
        .Where(x => x.Name.Contains('Enis'))
        .ToPagedListAsync(pageIndex,pageSize);
 
```


## View Usage

```
PM> ViewImport.cshtml
```

```csharp
    @addTagHelper *, ENPAGER 
```

```
PM> Using FrontEnd Pager {Using View}.cshtml
```

```csharp
      <EnPager total-items="Model.customer.TotalCount"
          total-pages="Model.customer.TotalPages"
          page="Model.customer.PageIndex"
          page-size="Model.customer.PageSize"
          show-active-link="true"
      />

```
     
     

 
