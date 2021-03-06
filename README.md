# NetMoney

[![nuget](https://img.shields.io/badge/nuget-v0.0.7-orange.svg)](https://www.nuget.org/packages/NetMoney/)
[![Build status](https://ci.appveyor.com/api/projects/status/aftis2sh7g6thse9?svg=true)](https://ci.appveyor.com/project/Djangoum/netmoney)

NetMoney is an open source library focused on making currency exchange rating an easy and safe task for .NET developers. 

NetMoney provides provides a thread safe communication environment with the [Fixer.io](http://fixer.io/) open source API using the [Circuit Breaker](https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker) pattern and thread safe code.

[Fixer.io](http://fixer.io/) is an open source API, [Fixer.io](http://fixer.io/) data is obtained from the [ECB](http://www.ecb.europa.eu/) public API.

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

**Adding and substraction**

NetMoney let you add and substract currencies. To add and substract currencies you are not asked to do a previous conversion, NetMoney do it for you only if it's necesary.

```
using NetMoney;
using NetMoney.Core;

//Must be treated as a singleton
Money moneySingleton = new Money(10, 5);

IConvertedCurreny australianDolars = await moneySingleton.From(Currency.EUR, 1000m).To(Currency.AUD);
IConvertedCurrency yuans = await moneySingleton.From(Currency.USD, 1000m).To(Currency.CNY);

australianDolars = australianDolars.Sum(yuans);

australianDolars = australiaDolars.Substract(yuans);
```

## Authors

* **Ariel Amor García** - *Main Contributor* - [Ariel Amor](https://github.com/Djangoum)

See also the list of [contributors](https://github.com/Djangoum/NetMoney/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/Djangoum/NetMoney/blob/master/LICENSE) file for details