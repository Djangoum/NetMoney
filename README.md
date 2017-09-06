# NetMoney

[![Scrutinizer Build](https://img.shields.io/scrutinizer/build/g/filp/whoops.svg)](https://www.nuget.org/packages/NetMoney/)
[![nuget](https://img.shields.io/badge/nuget-v0.0.5-orange.svg)](https://www.nuget.org/packages/NetMoney/)

NetMoney is an open source library focused on making currency exchange rating an easy and safe task for .NET developers. 

NetMoney provides provides a thread safe communication environment with the [Fixer.io](http://fixer.io/) API using the [Circuit Breaker](https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker) pattern and thread safe code.

NetMoney consumes the [Fixer.io](http://fixer.io/) API to have daily updated data. [Fixer.io](http://fixer.io/) is an open source API, [Fixer.io](http://fixer.io/) data is obtained from the [ECB](http://www.ecb.europa.eu/) public API.

### Installing

Easy Installing through Nuget package manager: 
```
Install-Package NetMoney -Version 0.0.5
```

```
dotnet add package NetMoney --version 0.0.5
```

### Usage

The main class of NetMoney is **Money** wich must be treated as a singleton. The method that let you retrieve exchange rates is *GetExchangeRatesAsync*  

**Usage Example**

```
using NetMoney;
using NetMoney.Core;

//Must be treated as a singleton
Money moneySingleton = new Money(10, 5);

await moneySingleton.GetExchangeRatesAsync(Currency.EUR, Currency.AUD, Currency.CNY, Currency.EUR);
```

**Fluent Api Usage**

NetMoney also provides a simple fluent api syntax sugar for those who feel more comfortable with this syntax.

```
using NetMoney;
using NetMoney.Core;

//Must be treated as a singleton
Money moneySingleton = new Money(10, 5);

await moneySingleton.From(Currency.EUR, 1000m).To(Currency.DKK);

// with date filtering
await moneySingleton.From(Currency.EUR, 1000m).FromDate(DateTime.Now).To(Currency.DKK);
```

## Authors

* **Ariel Amor García** - *Main Contributor* - [Ariel Amor](https://github.com/Djangoum)

See also the list of [contributors](https://github.com/Djangoum/NetMoney/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/Djangoum/NetMoney/blob/master/LICENSE) file for details