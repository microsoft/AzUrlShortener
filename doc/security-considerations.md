# Security Responsibilities

This is open-source software and delivered as a Proof-of-concept. Please consider which security approaches is appropriate for your use case.

## Current Implementation 

The [TinyBlazorAdmin](../src/TinyBlazorAdmin/) is secured using the built-in authentication feature of Azure Container Apps (ACA) is a simple and powerful way to add authentication your applications with minimal effort. Here are some key points to remember:

- You don't need to change the existing app code to add this authentication feature.
- This built-in authentication feature of ACA protects your entire application, not individual pages.

For more details about the built-in authentication feature of ACA, see [Authentication and authorization in Azure Container Apps](https://learn.microsoft.com/azure/container-apps/authentication).

## Basic Security Approaches

Using Azure Container Apps (ACA), the API container will only be accessible from the TinyBlazorAdmin and won't be exposed to the Internet. As a bonus, since TinyBlazorAdmin and the API are now running inside containers, you could also decide to run them locally.

The storage access got also a security upgrade. Instead of using a connection string, I will be using a Managed Identity to access the Azure Storage Table. This is a much more secure way to access Azure resources, and thanks to .NET Aspire, it is also very easy to implement.

For more details about Security read the [SECURITY.md](../SECURITY.md) file.