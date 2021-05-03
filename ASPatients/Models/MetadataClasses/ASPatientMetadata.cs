using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ASClassLibrary;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPatients.Models
{
    [ModelMetadataType(typeof(ASPatientMetadata))]
    public partial class Patient : IValidatableObject
    {

        public readonly PatientsContext _context = new PatientsContext();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == null)
            {
                FirstName = string.Empty;
            }
            else
            {
                FirstName = FirstName.Trim();
                FirstName = ASValidations.ASCapitalize(FirstName).ToString();
            }

            if (LastName == null)
            {
                LastName = string.Empty;
            }
            else
            {
                LastName = LastName.Trim();
                LastName = ASValidations.ASCapitalize(LastName).ToString();
            }

            if (Gender == null)
            {
                Gender = string.Empty;
            }
            else
            {
                Gender = Gender.Trim();
                Gender = ASValidations.ASCapitalize(Gender).ToString();
            }

            if (Address == null)
            {
                Address = string.Empty;
            }
            else
            {
                Address = Address.Trim();
                Address = ASValidations.ASCapitalize(Address).ToString();
            }

            if (City == null)
            {
                City = string.Empty;
            }
            else
            {
                City = City.Trim();
                City = ASValidations.ASCapitalize(City).ToString();
            }

            if(ProvinceCode != null)
            {
                var provSearch = _context.Province.Where(p => p.ProvinceCode == ProvinceCode).FirstOrDefault();

                if (provSearch == null)
                {
                    yield return new ValidationResult("Province code is not on file", new[] { nameof(ProvinceCode) });
                    yield return new ValidationResult("Province code is required to validate Postal Code", new[] { nameof(PostalCode) });
                }
                else
                {
                    var countrySearch = _context.Country.Where(a => a.CountryCode == provSearch.CountryCode).FirstOrDefault();

                    if (provSearch.CountryCode == "CA")
                    {
                        ProvinceCode = ProvinceCode.Trim();
                        ProvinceCode = ProvinceCode.ToUpper();

                        if (PostalCode != null || PostalCode == "")
                        {
                            PostalCode = PostalCode.Trim().ToUpper();
                            if (provSearch.FirstPostalLetter.Contains(PostalCode.Substring(0, 1)) == false)
                                yield return new ValidationResult("First letter of postal code not valid for given province", new[] { nameof(PostalCode) });
                            else
                            {
                                if (ASValidations.ASPostalCodeValidation(PostalCode) == false)
                                    yield return new ValidationResult("Postal code not in cdn format: A3A 3A3", new[] { nameof(PostalCode) });
                                else
                                {
                                    PostalCode = string.Format("{0} {1}", PostalCode.Substring(0, 3), PostalCode.Substring(3));
                                }
                            }
                        }

                    }
                    else if (provSearch.CountryCode == "US")
                    {
                        ProvinceCode = ProvinceCode.Trim();
                        if (!String.IsNullOrEmpty(PostalCode))
                        {
                            PostalCode = PostalCode.Trim();
                            if (!ASValidations.ASZipCodeValidation(PostalCode))
                            {
                                yield return new ValidationResult("US Zip code format incorrect: 12345 / 12345-6789", new[] { nameof(PostalCode) });
                            }
                        }


                    }
                }
            }
            
            

            if ((Deceased == true) && (DateOfDeath == null))
                yield return new ValidationResult("If deceased is true, a date of death is required", new[] { nameof(DateOfDeath) });
            else if ((Deceased == false) && (DateOfDeath != null))
                yield return new ValidationResult("Deceased must be true, if Date of Death is provided", new[] { nameof(Deceased) });

            if(Ohip != null)
                Ohip = Ohip.ToUpper();

            if(HomePhone != null)
            {
                HomePhone = HomePhone.Trim();
                var HomePhoneNumber = ASValidations.ASExtractDigits(HomePhone).ToString();
                if (HomePhoneNumber.Length != 10)
                {
                    yield return new ValidationResult("Home Phone if provided, should be 10 digits: 123-123-1234", new[] { nameof(HomePhone) });
                }
                else
                {
                    HomePhone = string.Format("{0}-{1}-{2}", HomePhoneNumber.Substring(0, 3), HomePhoneNumber.Substring(3, 3), HomePhoneNumber.Substring(6));

                }
            }
            


            yield return ValidationResult.Success;

        }
    }

    public class ASPatientMetadata
    {
        public int PatientId { get; set; }

        [Display(Name ="First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Street Address")]
        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        [Display(Name = "Province Code")]
        public string ProvinceCode { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "OHIP")]
        [RegularExpression(@"^\d{4}-\d{3}-\d{3}-[A-Za-z]{2}$", ErrorMessage = "OHIP, if provided, must match pattern: 1234-123-123-XX")]
        public string Ohip { get; set; }

        [Remote("dobValidation", "Remote")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public bool Deceased { get; set; }

        [Remote("dodValidation", "Remote")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [Display(Name = "Date of Death")]
        public DateTime? DateOfDeath { get; set; }

        [Display(Name = "Home Phone")]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="Home Phone if provided, should be 10 digits: 123-123-1234")]
        public string HomePhone { get; set; }

        [Required]
        [RegularExpression(@"^[MmFfXx]{1}$", ErrorMessage = "Gender must be either 'M' 'F' or 'X'")]
        public string Gender { get; set; }
    }
}