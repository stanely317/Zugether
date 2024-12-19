using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Zugether.Models;

namespace Zugether.Controllers
{
	public class HomeController : Controller
	{
		private readonly ZugetherContext _context;
		private readonly PasswordHasher<Member> _passwordHasher;
		public HomeController(ZugetherContext context)
		{
			_context = context;
			_passwordHasher = new PasswordHasher<Member>();
		}
		public string? Userforget_Email { get; set; }

		public IActionResult Index()
		{
			ViewBag.memberName = HttpContext.Session.GetString("memberName") ?? HttpContext.Session.GetString("googleName") ?? "";
			ViewBag.memberID = HttpContext.Session.GetString("memberID") ?? HttpContext.Session.GetString("googleMemberID") ?? "";
			ViewBag.memberEmail = HttpContext.Session.GetString("memberEmail") ?? HttpContext.Session.GetString("googleEmail") ?? "";
			if (!string.IsNullOrEmpty(ViewBag.memberName) && !string.IsNullOrEmpty(ViewBag.memberID) && !string.IsNullOrEmpty(ViewBag.memberEmail))
			{
				HttpContext.Session.SetString("isLogin", "true");
			}
			else
			{
				HttpContext.Session.SetString("isLogin", "false");
			}

			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		// �p���ڭ�
		public IActionResult Contact()
		{
			ViewBag.show = false;
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Contact(Contact_us contact)
		{
			_context.Contact_us.Add(contact);
			await _context.SaveChangesAsync();

			ViewBag.color = "success";
			ViewBag.show = true;
			ViewBag.message = "�P�±z���d���A�w�N�d���ǰe���޲z��";
			return View();
		}

		/* ���U */
		public IActionResult Register()
		{
			var googleName = HttpContext.Session.GetString("googleName");
			var googleEmail = HttpContext.Session.GetString("googleEmail");

			if (!string.IsNullOrEmpty(googleName) && !string.IsNullOrEmpty(googleEmail) && TempData["checkGoogle"] != null && (bool)TempData["checkGoogle"])
			{
				ViewBag.googleName = googleName;
				ViewBag.googleEmail = googleEmail;
				ViewBag.googleLogin = true;
			}
			ViewBag.show = false;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(Member register)
		{
			var r = (from m in _context.Member
					 where m.email == register.email
					 select m).SingleOrDefault();
			//�P�_�H�c�O�_�Q���U
			if (r != null)
			{
				ViewBag.color = "danger";
				ViewBag.show = true;
				ViewBag.message = "Email �w�Q���U !";
				ViewBag.registerfalse = false;
				return View(register);
			}
			if (register.password != null)
			{
				//�N�K�X�ন����
				register.password = _passwordHasher.HashPassword(register, register.password!);
			}
			_context.Member.Add(register);
			await _context.SaveChangesAsync();
			ViewBag.color = "success";
			ViewBag.show = true;
			ViewBag.message = "���U���\";
			return View();
		}
		//�n�J
		[HttpPost]
		public async Task<IActionResult> Login(Member userlogin)
		{
			Member? em = await _context.Member.FirstOrDefaultAsync(u => u.email == userlogin.email);
			//�P�_�H�c�O�_�Q���U
			if (em == null)
			{
				TempData["logincolor"] = "danger";
				TempData["loginshow"] = true;
				TempData["loginmessage"] = "Email ���Q���U !";
				return RedirectToAction("Index", "Home");
			}
			//�P�_�K�X�O�_���T(�ϥ�����)
			var result = _passwordHasher.VerifyHashedPassword(em, em.password!, userlogin.password!);
			if (result == PasswordVerificationResult.Success)
			{
				TempData["UserName"] = em.name.ToString();

				//���ե�
				TempData["testt"] = em.name.ToString();

				// �K�X���Ҧ��\�A�i��n�J
				HttpContext.Session.SetString("memberID", em.member_id.ToString());//�ϥΪ�ID
				HttpContext.Session.SetString("memberName", em.name.ToString());//�ϥΪ̦W��
				HttpContext.Session.SetString("memberEmail", em.email.ToString());//�ϥΪ�Email

				TempData["logincolor"] = "success";
				TempData["loginshow"] = true;
				TempData["loginmessage"] = "�n�J���\ !";
				return RedirectToAction("Index", "Home");
			}
			//�K�X��J���~
			TempData["logincolor"] = "danger";
			TempData["loginshow"] = true;
			TempData["loginmessage"] = "�K�X���~ !";

			return RedirectToAction("Index", "Home");
		}
		//�n�X
		public IActionResult Logout()
		{
			// �M���Ҧ� Session ���
			HttpContext.Session.Clear();

			// ���s�ɦV�ܭ���
			return RedirectToAction("Index", "Home");
		}


		//�ѰO�K�X����Email
		[HttpPost]
		public async Task<IActionResult> CheckEmail([FromBody] EmailRequest request)
		{
			var exists = await _context.Member.FirstOrDefaultAsync(m => m.email == request.Email);

			return Json(new { exists });
		}

		//�ѰO�K�X���]�K�X
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(EmailRequest userforget)
		{

			var member_exist = (from m in _context.Member
								where m.email == userforget.Email
								select m).SingleOrDefault();

			if (member_exist != null)
			{
				member_exist.password = _passwordHasher.HashPassword(member_exist, userforget.ResetPassword!);
				_context.Update(member_exist);
				await _context.SaveChangesAsync();
				TempData["logincolor"] = "success";
				TempData["loginshow"] = true;
				TempData["loginmessage"] = "�K�X�ܧ󦨥\ !";
				return RedirectToAction("Index", "Home");
			}
			TempData["logincolor"] = "danger";
			TempData["loginshow"] = true;
			TempData["loginmessage"] = "���~�I";
			return RedirectToAction("Index", "Home");

		}
		public class EmailRequest
		{
			public string? Email { get; set; }
			public string? ResetPassword { get; set; }
			public string? CheckResetPw { get; set; }

		}
		public IActionResult GoogleLogin()
		{
			var redirectUrl = Url.Action("GoogleResponse", "Home");
			return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "Google");
		}
		public async Task<IActionResult> GoogleResponse()
		{
			// ���token
			var user = HttpContext.User;
			// ����������T
			var googleName = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? "no name";
			var googleEmail = user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value ?? "no email";
			var query = await _context.Member.FirstOrDefaultAsync(x => x.email == googleEmail.Trim());
			if (query != null)
			{
				HttpContext.Session.SetString("googleName", query.name);
				HttpContext.Session.SetString("googleEmail", query.email);
				HttpContext.Session.SetString("googleMemberID", query.member_id.ToString());
				HttpContext.Session.SetString("checkGoogleLogin", "true");
				return RedirectToAction("Index");
			}
			HttpContext.Session.SetString("googleName", googleName);
			HttpContext.Session.SetString("googleEmail", googleEmail);
			TempData["checkGoogle"] = true;
			return RedirectToAction("Register", "Home");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		//Alert�ʵe
		public IActionResult Alert(string color, string alertText, bool show, int time)
		{
			var model = new PartialAlert
			{
				Color = color,
				AlertText = alertText,
				Show = show,
				Time = time
			};
			return PartialView("_PartialAlert", model);
		}
	}
}
