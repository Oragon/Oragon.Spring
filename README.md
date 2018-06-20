# Summer.Net
A .NetStandard 2.0 port of Spring.Net

### This port extends [Spring.NET  2.0](https://github.com/spring-projects/spring-net) adding support to .NET Standard

I'm Luiz Carlos Faria, Spring.NET user. I love this project. I couldn't just and watch the project die, Spring.NET solves many questions and adress many ways to do IoC and Dependency Injection in the right way.
It's a pleasure to contribute with this discussion.


## Goals
A main goal with this port is solve questions about adapt and overcome migration from .NET Framework to .NET Standard. Only core projects of Spring.NET will stay here. 


Understanding what's necessary to develop a framework in 2018, change all kind of decisions taken in the past. Spring.NET is largest project that i know, with more and more responsabilities in only one repository, distributed on some assemblies.


This choice does not sounds good for 2018. Squeeze the Spring.NET responsabilities may improve community adoption, just causing needs that community will respond with new framework extensions. Challenges about best extension for .NET Core configuration model, MVC, and others skill must be done.

In order of that, this port/fork contains only two projects:

### Spring.Core
* A full featured Inversion of Control container
* Expression Language for lightweight scripting

### Spring.AOP
* An Aspect Oriented Programming framework

### Merging both
I really considering moving Spring.AOP to Spring.Core, aggregating both in only one project/assembly/nuget package.  The segregation of both projects does not sounds good for me. 


# Issues

This project was created to present for Spring.NET developers that is possible to migrate. It's sounds like: 

_"Hey there, why nobody starts a port? We can do that! It's not simple, but you don't to spend many time, come on, don't give up!"_

## Still incubating

For example look that:
- [Summer.Core.Fakes/System.Runtime.Remoting.cs](https://github.com/luizcarlosfaria/summer-net/blob/netstandard--2-0/Summer.Core/__fakes/System.Runtime.Remoting.cs)
- These file address not resolved dependencies.
- The default configuration model, based on ConfigurationManager is not working.

# Examples

## Factory / DI Example
![spring net-on-dotnetstandard2-docker](http://res.cloudinary.com/luizcarlosfaria/image/upload/v1506499371/SPRING.NET/spring.net-on-dotnetstandard2-docker_xswvba.png)

## Factory / DI / AOP Example
![spring net-on-dotnetstandard2-aop](http://res.cloudinary.com/luizcarlosfaria/image/upload/v1506499371/SPRING.NET/spring.net-on-dotnetstandard2-aop_lif2uw.png)


## See more about on Spring.NET project

https://github.com/spring-projects/spring-net/issues/133

https://github.com/spring-projects/spring-net/issues/144


:warning: **It's a proposal, only Spring.Core and Spring.Aop projects was ported to .NET Standard. Other projects does not will be ported on this fork.** :warning:




© [Luiz Carlos Faria](http://gago.io/) ![made in brazil][brazil]


[brazil]:http://www.goal.cc/content/images/flags/28.png