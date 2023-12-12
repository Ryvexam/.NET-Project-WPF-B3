using System;
using System.ComponentModel.DataAnnotations;
using MyErp.Base;
using MyErp.Views;

namespace MyErp.Entities;

public class ClientEntity : ViewEntitiesBase
{
    private string? _siretNumber;
    private string? _companyName;
    private string? _firstName;
    private string? _lastName;
    private DateTime _createdDate;
    private string? _city;
    private string? _postalCode;
    private string? _phoneNumber;
    private bool _isEnabled;
    private bool _shouldShown;
    private bool _mustShow;
    private string _libelle;

    
    public bool MustShow
    {
        get => _mustShow;
        set => SetProperty(ref _mustShow, value);
    }

    public bool ShouldShown
    {
        get => _shouldShown;
        set => SetProperty(ref _shouldShown, value);
    }

    public string FullName => LastName +" "+ FirstName;
    
    public string Libelle => string.IsNullOrEmpty(CompanyName) ? FullName : CompanyName;
    public string SiretNumber
    {
        get => _siretNumber;
        set => SetProperty(ref _siretNumber, value);
    }
    public string? CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
    }
    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }
    public string PostalCode
    {
        get => _postalCode;
        set => SetProperty(ref _postalCode, value);
    }
    public string FirstName
    {
        get => _firstName;
        set
        {
            SetProperty(ref _firstName, value);
            OnPropertyChanged(nameof(FullName));
        }
    }
    public string? LastName
    {
        get => _lastName;
        set
        {
            SetProperty(ref _lastName, value);
            OnPropertyChanged(nameof(FullName));
        }
    }

   public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }
    public DateTime CreatedDate
    {
        get => _createdDate;
        set => SetProperty(ref _createdDate, value);
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }
    

    public override string ToString()
    {
        return FullName;
    }
}