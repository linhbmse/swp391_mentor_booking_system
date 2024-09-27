using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.DTOs.ImportedUser;
using SwpMentorBooking.Infrastructure.Utils;
using SwpMentorBooking.Web.ViewModels;

namespace SwpMentorBooking.Web.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CSVFileService _csvFileService;
        private readonly AutoMapperService _autoMapperService;
        private readonly PasswordGeneratorService _passwordGeneratorService;
        public AdministratorController(IUnitOfWork unitOfWork, CSVFileService csvFileService,
               AutoMapperService autoMapperService, PasswordGeneratorService passwordGeneratorService)
        {
            _unitOfWork = unitOfWork;
            _csvFileService = csvFileService;
            _autoMapperService = autoMapperService;
            _passwordGeneratorService = passwordGeneratorService;
        }

        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetAll();
            return View(users);
        }

        public IActionResult Import()
        {
            ImportUserTypeVM importUserTypeVM = new ImportUserTypeVM()
            {
                UserType = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Student", Value = "Student" },
                    new SelectListItem() { Text = "Mentor", Value = "Mentor" }
                }
            };
            return View(importUserTypeVM);
        }
        [HttpPost]
        public IActionResult Import(ImportUserTypeVM userTypeVM)
        {
            // Get selected user type & CSV file input
            string userType = userTypeVM.SelectedUserType;
            var inputFile = userTypeVM.InputFile;

            // Uploaded file is empty
            if (inputFile is null || inputFile.Length == 0)
            {
                return View();
            }
            // Try to read CSV file
            try
            {
                // Save the file temporarily
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    inputFile.CopyTo(stream);
                }

                // Dictionary to map user types to their corresponding DTO types
                var userTypeToDtoType = new Dictionary<string, Type>
                {
                    { "Student", typeof(CSVStudentDTO) },
                    { "Mentor", typeof(CSVMentorDTO) }
                };

                // Get the corresponding DTO type for the selected user type
                if (!userTypeToDtoType.TryGetValue(userType, out var dtoType))
                {
                    throw new InvalidOperationException("Invalid user type selected.");
                }

                // Use reflection to call the ReadCSVFile method with the appropriate type
                var method = typeof(CSVFileService).GetMethod("ReadCSVFile").MakeGenericMethod(dtoType);
                var importedData = method.Invoke(_csvFileService, new object[] { filePath });

                // Validate the imported data
                var validationResults = _csvFileService.ValidateCSVData((dynamic)importedData);

                // Delete the temporary file
                System.IO.File.Delete(filePath);

                /// ImportUserPreview currently contains CSVStudentDTO
                // Use ImportResultVM to pass the validation errors to the View
                var importResult = new ImportUserPreviewVM
                {
                    Results = validationResults
                };

                TempData["ImportResult"] = JsonConvert.SerializeObject(importResult);
                return RedirectToAction(nameof(ImportConfirm));
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public IActionResult ImportConfirm()
        {
            var importResult = TempData["ImportResult"] as string;
            if (string.IsNullOrEmpty(importResult))
            {
                return RedirectToAction(nameof(Import));
            }
            var resultList = JsonConvert.DeserializeObject<ImportUserPreviewVM>(importResult);
            return View(resultList);
        }

        [HttpPost]
        public IActionResult ImportConfirm(string DTOJson)
        {
            if (string.IsNullOrEmpty(DTOJson))
            {
                return RedirectToAction(nameof(Import));
            }

            var dtos = JsonConvert.DeserializeObject<List<CSVStudentDTO>>(DTOJson);

            if (dtos == null || !dtos.Any())
            {
                return RedirectToAction(nameof(Import));
            }

            try
            {
                IEnumerable<User> users = _autoMapperService.MapAll<CSVStudentDTO, User>(dtos);

                foreach (var user in users)
                {
                    user.Password = _passwordGeneratorService.GeneratePassword();
                    _unitOfWork.User.Add(user);
                }
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
