using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraduationProject022.Data;
using GraduationProject022.Models;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using NToastNotify;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject022.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        public ProfilesController(ApplicationDbContext context,IToastNotification toastNotification)
        {
            _context = context;
            this._toastNotification = toastNotification;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profile.ToListAsync());
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
                return NotFound();

            string content = $"الاسم بالكامل :\n\t\t{profile.FullName}\nالبريد الالكتروني :\n\t\t{profile.Email}\nالمؤهل الدراسي :\n\t\t{profile.Qualification}\nرقم الهاتف : {profile.PhoneNo}\nالرقم القومي : {profile.NationalNo}\nتاريخ الميلاد : {profile.Age}\nلقاح كرونا : {(profile.IsVaccine ? "محصن" : "غير محصن")}\nالعنوان :\n\t\t{profile.Address}.";//\nمعلومات اضافية :\n\t\t\t{profile.ExtraInfo}
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
                QRCode qRCode = new QRCode(qRCodeData);
                Bitmap bmp = qRCode.GetGraphic(20);
                //if you want to add logo on QR Code 
               // string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Ab.jpg");
               // Bitmap bmp = qRCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(path));
                bmp.Save(ms, ImageFormat.Png);
                ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
            return View(profile);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Qualification,Address,PhoneNo,NationalNo,ExtraInfo,Age,IsVaccine,ProfilePicture")] Profile profile)
        {
            ValidationDublicate(profile);

            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();
                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    profile.ProfilePicture = dataStream.ToArray();
                }
                _context.Add(profile);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("تمت الاضافة بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Qualification,Address,PhoneNo,NationalNo,ExtraInfo,Age,IsVaccine,ProfilePicture")] Profile profile)
        {
            if (id != profile.Id)
                return NotFound();

            ValidationDublicate(profile);

            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using var dataStream = new MemoryStream();
                        await file.CopyToAsync(dataStream);
                        profile.ProfilePicture = dataStream.ToArray();
                    }
                    else
                    {
                        var entity = await _context.Profile.AsNoTracking().FirstOrDefaultAsync(a => a.Id == profile.Id);
                        profile.ProfilePicture = entity.ProfilePicture;
                    }
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toastNotification.AddSuccessToastMessage("تمت التعديل بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var profile = await _context.Profile.FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
                return NotFound();

            _context.Profile.Remove(profile);
            _context.SaveChanges();
            return Ok();
        }

        private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }


        private  IActionResult ValidationDublicate(Profile profile)
        {
            if(profile.Id == 0)
            {
                if (_context.Profile.Any(e => e.Email == profile.Email))
                {
                    ModelState.TryAddModelError("Email", "هذا البريد الالكتروني موجود بالفعل");
                    return View(profile);
                }
                else if (_context.Profile.Any(e => e.PhoneNo == profile.PhoneNo))
                {
                    ModelState.TryAddModelError("PhoneNo", " رقم الهاتف هذا موجود بالفعل");
                    return View(profile);
                }
                else if (_context.Profile.Any(e => e.NationalNo == profile.NationalNo))
                {
                    ModelState.TryAddModelError("NationalNo", " هذا الرقم القومي موجود بالفعل");
                    return View(profile);
                }
            }
            else
            {
                if (_context.Profile.Any(e => e.Email == profile.Email && e.Id != profile.Id))
                {
                    ModelState.TryAddModelError("Email", "هذا البريد الالكتروني موجود بالفعل");
                    return View(profile);
                }
                else if (_context.Profile.Any(e => e.PhoneNo == profile.PhoneNo && e.Id != profile.Id))
                {
                    ModelState.TryAddModelError("PhoneNo", " رقم الهاتف هذا موجود بالفعل");
                    return View(profile);
                }
                else if (_context.Profile.Any(e => e.NationalNo == profile.NationalNo && e.Id != profile.Id))
                {
                    ModelState.TryAddModelError("NationalNo", " هذا الرقم القومي موجود بالفعل");
                    return View(profile);
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
