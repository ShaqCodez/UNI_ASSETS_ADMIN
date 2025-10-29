using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        private readonly IRepositoryWrapper repository;
        private readonly IWebHostEnvironment environment;

        public AssetController(IRepositoryWrapper repository, IWebHostEnvironment environment)
        {
            this.repository = repository;
            this.environment = environment;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Assets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Asset asset, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Create unique file name
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Save relative path to DB
                    asset.ImageUrl = "/Images/" + uniqueFileName;
                }
               asset.AssetId = GetAssetId();

                repository.AssetRepository.Create(asset);
                repository.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(asset);
        }
        string GetAssetId()
        {
            int assetCount = repository.AssetRepository.GetAll().Count()+1;
            if(assetCount > 9)
            {
                return $"0{assetCount}";
            }
                return $"00{assetCount}";
        }
        public IActionResult Index()
        {
            var assets = repository.AssetRepository.GetAll().ToList();
            return View(assets);
        }
        [HttpGet]
        public  IActionResult Edit(string id)
        {
            if (id == null)
                return NotFound();

            var asset =repository.AssetRepository.GetById(id);
            if (asset == null)
                return NotFound();

            return View(asset);
        }

        // POST: Assets/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(string id, Asset asset, IFormFile? ImageFile)
        {
            if (id != asset.AssetId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAsset = repository.AssetRepository.GetById(id);
                    if (existingAsset == null)
                        return NotFound();

                    // Update basic info
                    existingAsset.Name = asset.Name;
                    existingAsset.Description = asset.Description;
                    existingAsset.Default_Location = asset.Default_Location;

                    // Handle new image upload (replace old)
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(existingAsset.ImageUrl))
                        {
                            string oldFilePath = Path.Combine(environment.WebRootPath, existingAsset.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Save new image
                        string uploadsFolder = Path.Combine(environment.WebRootPath, "Images");
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        existingAsset.ImageUrl = "/Images/" + uniqueFileName;
                    }

                    repository.AssetRepository.Update(existingAsset);
                    repository.Save();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            return View(asset);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var asset = repository.AssetRepository.GetById(id);
            if (asset == null)
                return NotFound();

            // Delete image file if it exists
            if (!string.IsNullOrEmpty(asset.ImageUrl))
            {
                string filePath = Path.Combine(environment.WebRootPath, asset.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

           repository.AssetRepository.Delete(asset);
            repository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
