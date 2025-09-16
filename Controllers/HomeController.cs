using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u22710362_HW2.Models;

namespace u22710362_HW2.Controllers
{
    public class HomeController : Controller
    {
        public static String typeFilter = "All";
        public static String breedFilter = "All";
        public static String locationFilter = "All";

        private DefaultDataService dataService = new DefaultDataService();
        public ActionResult Index()
        {
            PetRecordsVM petRecords = new PetRecordsVM
            {
                Pets = dataService.getAdoptedPets(),
                TotalAdoptions = dataService.getTotalAdoptions(),
                TotalDonations = dataService.getTotalDonations()
            };

            return View(petRecords);
        }
        public ActionResult Pets()
        {
            PetRecordsVM petRecords = new PetRecordsVM
            {
                Pets = dataService.getFilteredPets(typeFilter, breedFilter, locationFilter),
                PetTypes = dataService.getAllPetTypes(),
                PetBreeds = dataService.getAllPetBreeds(),
                Locations = dataService.getAllLocations()
            };

            ViewBag.CurrentTypeFilter = typeFilter;
            ViewBag.CurrentBreedFilter = breedFilter;
            ViewBag.CurrentLocationFilter = locationFilter;

            return View(petRecords);
        }
        public ActionResult Adopt(int id)
        {
            Pet pet = dataService.getPetById(id);
            if (pet == null || pet.Status == "Adopted")
            {
                return RedirectToAction("Pets");
            }

            PetRecordsVM petRecords = new PetRecordsVM
            {
                Users = dataService.getAllUsers()
            };

            ViewBag.Pet = pet;
            return View(petRecords);
        }
        public ActionResult PostPet()
        {
            PetRecordsVM petRecords = new PetRecordsVM
            {
                Users = dataService.getAllUsers(),
                PetTypes = dataService.getAllPetTypes(),
                PetBreeds = dataService.getAllPetBreeds(),
                Locations = dataService.getAllLocations()
            };

            return View(petRecords);
        }
        public ActionResult Donate()
        {
            PetRecordsVM petRecords = new PetRecordsVM
            {
                Users = dataService.getAllUsers(),
                TotalDonations = dataService.getTotalDonations()
            };

            return View(petRecords);
        }
        public ActionResult SetPetFilter(String petType, String breed, String location)
        {
            HomeController.typeFilter = petType ?? "All";
            HomeController.breedFilter = breed ?? "All";
            HomeController.locationFilter = location ?? "All";
            return RedirectToAction("Pets");
        }

        public ActionResult ClearFilters()
        {
            HomeController.typeFilter = "All";
            HomeController.breedFilter = "All";
            HomeController.locationFilter = "All";
            return RedirectToAction("Pets");
        }
        [HttpPost]
        public ActionResult AdoptPet(int petId, int userId)
        {
            dataService.adoptPet(petId, userId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CreatePet(string name, int petTypeId, int breedId, int locationId, int age, decimal weight, string gender, string petStory, int postedByUserId, string b64Image)
        {
            if (string.IsNullOrEmpty(b64Image))
            {
                b64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwCdABmX/9k=";
            }

            dataService.postPet(name, petTypeId, breedId, locationId, age, weight, gender, petStory, postedByUserId, b64Image);
            return RedirectToAction("Pets");
        }
        [HttpPost]
        public ActionResult MakeDonation(int userId, decimal amount)
        {
            dataService.makeDonation(userId, amount);
            return RedirectToAction("Donate");
        }
    }
}