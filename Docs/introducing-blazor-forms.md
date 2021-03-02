# Introducing Blazor Forms

Forms are a key component in any web application as they allow us to collect information from end-users.  This could range from collecting user details as part of a sign-up process through to a single textbox that allows users to leave comments or post messages in an application.

In this article we'll explain the benefits of Blazor forms, we'll look at their lifecycle and we'll explain the fundamental concepts used to create form-driven solutions using Blazor.

## Form Fundamentals

Let's assume that we are building a Contact form which has two fields (Name and Email) and a button to submit the form. The code required to do this in ASP.NET MVC would look similar to this.

```html
<form asp-controller="Home" asp-action="Register" method="post"> 
    <label asp-for="Contact.Name" class="form-label">Name</label> 
    <input asp-for="Contact.Name" class="form-control" type="text" /> 
    <span asp-validation-for="Contact.Name" class="text-danger"></span> 

    <label asp-for="Contact.Email" class="form-label">Email address</label> 
    <input asp-for="Contact.Email" class="form-control" /> 
    <span asp-validation-for="Contact.Email" class="text-danger"></span> 

    <button class="btn btn-primary mt-3" type="submit">Submit</button> 
</form>
```

The form defines an HTTP method and controller action to which the data is sent. For brevity, the Controller code which handles the form submission is omitted.

The form uses ASP.NET tag helpers to render the correct types of input controls and validation messages based on attributes configured on the underlying data model.

One problem with this solution is that when the form is submitted, a full page refresh is required to update the state of the page. This requires sending a full HTTP payload to the client and it interrupts the users' experience. For example, the newly loaded page may result in the scroll position changing thus requiring the user to scroll to where they left off!

Of course a full page refresh can be avoided using JavaScript and AJAX to send only the form data to the server and handling the HTTP response to update only a specific fragment of the page.  This is the approach offered by SPA solutions which simplify the job of developing interactive, client-centric solutions by abstracting common tasks such as managing the state of fragments of pages away from the developer.

As we move away from a pure server-based approach by introducing JavaScript, we need to bring in third-party libraries or write additional client-side logic to handle some concerns such as making HTTP requests and displaying validation errors to the user. As we deal with handling AJAX requests, we may also need to parse data types such as dates and numbers from JSON response messages.

With Blazor, developers can take advantage of the following benefits when developing form-based solutions:

* .NET data types used throughout the entire life of the request 
* No need for additional coded API endpoints  
* No need to write code to pass JSON between the front and back-end 
* Ability to reuse the same validation code on the server and client

## Introducing Blazor Forms

Writing a similar form in Blazor would look like this.

```csharp
<EditForm Model="@ContactModel" OnSubmit=@FormSubmitted> 
    <label for="@nameof(Contact.Name)" class="form-label">Name</label> 
    <InputText Class="form-control" @bind-Value="ContactModel.Name"></InputText> 

    <label for="@nameof(Contact.Email)" class="form-label">Email</label> 
    <InputText Class="form-control" @bind-Value="ContactModel.Email"></InputText> 

    <button class="btn btn-primary mt-3" type="submit">Submit</button> 
</EditForm> 

@code 
{ 
    public Contact ContactModel { get; set; } = new(); 

    void FormSubmitted()
    { 
        Logger.LogInformation($"Name: {ContactModel.Name}, Email: {ContactModel.Email}");

        // You could call your Web API endpoints or services here
        // Eg: await Http.SendJsonAsync(HttpMethod.Post, "/api/Contact/Create", ContactModel);
    }
}
```

The structure of the mark-up looks very similar for both the Blazor and MVC code. The key points to highlight in the Blazor code are: 

* **EditForm** defines a Blazor form which renders a `<form>` element under-the-hood. You could bind either a Model (a POCO) or an `EditContext` as the data that represents the form. More on this will be discussed later. 

* **Model** attribute lets us bind a top-level model object to the form. This model object is updated whenever we make changes to the form. 

* **OnSubmit** is a callback event that gets triggered when we submit the form. Blazor also provides us `OnValidSubmit` and `OnInvalidSubmit` convenience methods. 

* **InputText** is a Blazor form component which represents an `<input>` element. The Input element is annotated with the `@bind-Value` attribute to bind to the model. In our case it is `Model.Name` and `Model.Email`. By default the corresponding model field that's bound to this component is updated in the `onChangeEvent` handler. 

The Blazor @code block initializes our ContactModel object and has a method that gets triggered when the form is submitted. Note that all code is written using C# and we can even use .NET types such as HttpClient and take advantage of .NET dependency injection to help with separation of concerns and managing dependencies.

## Blazor Component Model

We can make use of standard HTML form and input elements to create a Blazor form. However, it would be cumbersome to maintain their events and manage the form state when it comes to validation. Therefore, Blazor provides us a suite of out-of-the-box components which makes form validation easier.

In the example above, we used the EditForm and InputText components to capture input and process the form. All Blazor form elements we have seen up to now, including the [EditForm](https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/EditForm.cs), inherit from an abstract class called [ComponentBase](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.componentbase?view=aspnetcore-5.0).

This means any form components participate in the component lifecycle can be used to extend functionality by providing our own implementation for things such as validation and component styling if need be. We will look at a whole variety of custom controls others have built leveraging this concept.

The default set of Blazor form components that map to standard HTML input controls as shown in the following table:

| Input component                                                                                                                                  | Rendered as&hellip;       |
| ------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------- |
| [`<InputCheckbox>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputcheckbox?view=aspnetcore-5.0)         | `<input type="checkbox">` |
| [`<InputDate>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputdate-1?view=aspnetcore-5.0)               | `<input type="date">`     |
| [`<InputFile>`](https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-5.0)                                            | `<input type="file">`     |
| [`InputNumber<TValue>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputnumber-1?view=aspnetcore-5.0)     | `<input type="number">`   |
| [`InputRadio`](https://docs.microsoft.com/en-us/aspnet/core/blazor/forms-validation?view=aspnetcore-5.0#radio-buttons)                           | `<input type="radio">`    |
| [`InputRadioGroup`](https://docs.microsoft.com/en-us/aspnet/core/blazor/forms-validation?view=aspnetcore-5.0#radio-buttons)                      | `<input type="radio">`    |
| [`InputSelect<TValue>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputselect-1?view=aspnetcore-5.0)     | `<select>`                |
| [`InputText`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputtext?view=aspnetcore-5.0)                   | `<input>`                 |
| [`InputTextArea`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.inputtextarea?view=aspnetcore-5.0)           | `<textarea>`              |


All of these input components provide two-way data binding which makes it easier to keep our UI components and the data objects in sync especially with form validations. Moreover, we can also specify any additional attributes that are not provided by Blazor, which will be rendered to the to the HTML page.

We also have the opportunity to use any of the third-party specialized controls that are available via NuGet to extend the functionality of our forms.  These include specialized components such as:

* [Blazored](https://github.com/Blazored) component library which includes controls that allow users to enter and view Markdown in forms
* [Telerik](https://www.telerik.com/blazor-ui)'s Blazor component is a comprehensive library of components that can be used to create very customized solutions
* [MudBlazor](https://mudblazor.com/) component library inspired by Google's Material design language and written entirely in C#

## Model Binding

When creating a new EditForm in Blazor, we must pass either a [Model](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.editform.model?view=aspnetcore-5.0#Microsoft_AspNetCore_Components_Forms_EditForm_Model) or an [EditContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.editcontext?view=aspnetcore-5.0) parameter into it. The descriptions of the two are as follows:

It is worth noting that you cannot pass both at the same time as it will throw an error if you do so.

Let's see how the EditForm gets created under the covers. The simplified version of this process is shown in the diagram below.

![](./images/creation-of-editform.png)

Now that we have an idea of how the EditForm is created, let's see how Blazor handles when you type in something in an input text component.

![](./images/editform-onchange.png)

In summary, what we need to understand here is that, whenever an `OnChange` event happens in any of the fields of a form, the EditContext will update. As a result, the EditForm's lifecycle events which are defined in ComponentBase will get called causing the Renderer class to re-render any changes.

## Validating Form Inputs

Now that we understand how a form is constructed in Blazor, we'll take a brief look at form validations. Let's build on top of our previous example and validate the input fields. Just as in ASP.NET MVC or Razor pages, we can use [Data Annotations](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0) to decorate our model parameters.

> Note: Make sure to add `Microsoft.AspNetCore.Mvc.DataAnnotations` NuGet package to your project if your models are on a .NET Standard library project.

We will add the following annotations to our model.

```csharp
public class Contact
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

All supported data annotations can be found [here](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0).

In the markup for the form, we previously configured the OnSubmit handler. This handler is triggered every time we submit the form whether it's valid or not. With Blazor forms, we can change this to OnValidSubmit to ensure that a user can only submit the form if validation passes.

```html
<EditForm Model="@ContactModel" OnValidSubmit=@FormSubmitted>
    ...
</EditForm>
```

![](./images/form-validation-1.png)

As you can see in the above image, the fields appear with a green border which indicates the form data is valid. This is not the case as the provided value for the Email field is clearly not a valid email address.

To address this issue, simply add a `<DataAnnotationsValidator>` element to the form.  This component provides validation checks for data attributes that have been configured on the model instance.

```html
<EditForm Model="@ContactModel" OnValidSubmit=@FormSubmitted>
    <DataAnnotationsValidator />
    ...
</EditForm>
```
Again, the `<DataAnnotationsValidator>` component inherits from `ComponentBase` and has access to the `EditContext`. During the initialization of this component, it will register two types of validators, one on the model-level another on the field-level. More on this can be found [here](https://github.com/dotnet/aspnetcore/blob/edc1ca88e17e6cb60a5ea0966d751075d35111b9/src/Components/Forms/src/EditContextDataAnnotationsExtensions.cs#L36).

If you check the UI now you will see the following.

![](./images/form-validation-2.png)

As we can see, the validations have kicked in and the Email input box is highlighted in red. We could also show a helpful summary of validation errors by using a ValidationSummary component at a desired position on the form.

![](./images/form-validation-3.png)

You can also choose to show validation messages alongside input controls by using `ValidationMessage` components. The code will be as follows:

```html
<EditForm Model="@ContactModel" OnValidSubmit=@FormSubmitted>
    <DataAnnotationsValidator />
    
    <label for="@nameof(Contact.Name)" class="form-label">Name</label>
    <InputText Class="form-control mb-2" @bind-Value="ContactModel.Name"></InputText>
    <ValidationMessage For="() => ContactModel.Email"></ValidationMessage>
    
    <label for="@nameof(Contact.Email)" class="form-label">Email</label>
    <InputText Class="form-control" @bind-Value="ContactModel.Email"></InputText>
    <ValidationMessage For="() => ContactModel.Email"></ValidationMessage>
    
    <button class="btn btn-primary mt-3" type="submit">Submit</button>
</EditForm>
```

![](./images/form-validation-4.png)

The following summary describes each of the validation components that Blazor provides.

| Validation Component	                                                                                                                                                                | Usage                                                                                           |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------- |
| [`<ValidationSummary>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.validationsummary?view=aspnetcore-5.0)                                      | Show validation error messages as a group at a set position on the page                         |
| [`<ValidationMessage>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.validationmessage-1?view=aspnetcore-5.0)                                    | Show validation error messages for an individual input element at set positions within the page |
| [`<DataAnnotationsValidator>`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.dataannotationsvalidator?view=aspnetcore-5.0)                        | Applies validation rules based on Data Annotations at runtime                                   |
| [`<ObjectGraphDataAnnotationsValidator>`](https://docs.microsoft.com/en-us/aspnet/core/blazor/forms-validation?view=aspnetcore-5.0#nested-models-collection-types-and-complex-types)  | Applies validation rules similar to DataAnnotationsValidator except that it traverses nested properties within an object hierarchy of the given model. Note that this need to be added as a separate dependency.                                                               |

## Conclusion

In this article we have given an overview of the fundamental concepts of Blazor forms. 

We compared ASP.NET MVC forms to Blazor forms, different form component Blazor provides, how EditForm works with the EditContext and lastly, how to perform form validations.