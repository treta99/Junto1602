using Junto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Junto.Controllers
{
    public class HomeController : Controller
    {
        private BdJunto db = new BdJunto();
        public ActionResult Index()
        {
            Usuario u = new Usuario();
            u.strNome = Session["nome"].ToString();
            return View(u);
        }

        [HttpGet]
        public ActionResult Login()
        {
            Session["nome"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Usuario u)
        {
            try
            {
                if (u.strSenha == null)
                {
                    ModelState.AddModelError("strSenha", "Insira uma senha!");
                    return View(u);
                }

                if (u.strMatricula.Length != 7)
                {
                    ModelState.AddModelError("strMatricula", "Insira uma matricula correta!");
                    return View(u);
                }
                if (u.strSenha.Length < 1)
                {
                    ModelState.AddModelError("strSenha", "Insira uma senha para login!");
                    return View(u);
                }

                using (BdJunto bd = new BdJunto())
                {
                    var v = bd.tbUsuario.Where(a => a.strMatricula.Equals(u.strMatricula) && a.strSenha.Equals(u.strSenha)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["usuID"] = v.idUsuario.ToString();
                        Session["nome"] = v.strNome.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("strMatricula", "Matricula ou Senha incorreta!");
                return View(u);
            }
            catch(Exception a)
            {
                ModelState.AddModelError("strMatricula", "Erro!");
                return View();
            }

        }
        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cadastro(Models.Usuario u)
        {
            try
            {
                if (u.strMatricula.Length != 7)
                {
                    ModelState.AddModelError("strMatricula", "Insira uma matricula correta!");
                    return View(u);
                }
                if (u.strSenha.Length < 1)
                {
                    ModelState.AddModelError("strSenha", "Insira uma senha!");
                    return View(u);
                }
                if (u.strEmail.Length < 1)
                {
                    ModelState.AddModelError("strEmail", "Insira um e-mail para cadastro!");
                    return View(u);
                }
                if (u.strNome.Length < 1)
                {
                    ModelState.AddModelError("strNome", "Insira um Nome para cadastro!");
                    return View(u);
                }

                if (ValidaEmail(u.strEmail) == false )
                {
                    ModelState.AddModelError("strEmail", "Insira um e-mail válido!");
                    return View(u);
                }



                tbUsuario usu = new tbUsuario();
                usu.strMatricula = u.strMatricula;
                usu.strEmail = u.strEmail;
                usu.strNome = u.strNome;
                usu.strSenha = u.strSenha;
                usu.dtCadastro = DateTime.Now;

                db.tbUsuario.Add(usu);
                db.SaveChanges();


                return RedirectToAction("Login", "Home");

            }
            catch (Exception a)
            {
                ModelState.AddModelError("strMatricula", "Erro no cadastro!");
                return View(u);
            }

        }
        [HttpGet]
        public ActionResult Editar()
        {
            Usuario u = new Usuario();

            string idG = Session["usuID"].ToString();

            int id;
            if (int.TryParse(idG, out id))

            if (id == null)
            {
                ModelState.AddModelError("strMatricula", "Não foi possivel carregar os dados!");
                return View(u);
            }

            tbUsuario usu = db.tbUsuario.Find(id);

            if (usu == null)
            {
                ModelState.AddModelError("strMatricula", "Não foi possivel carregar os dados!");
                return View(u);
            }

            u.idUsuario = (int)usu.idUsuario;
            u.strMatricula = usu.strMatricula;
            u.strNome = usu.strNome;
            u.strEmail = usu.strEmail;
            u.strSenha = "";

            return View(u);
        }
        [HttpPost]
        public ActionResult Editar(Models.Usuario u)
        {
            try
            {
                if(u.strSenha == null)
                {
                    ModelState.AddModelError("strSenha", "Insira uma senha!");
                    return View(u);
                }

                if (u.strMatricula.Length != 7)
                {
                    ModelState.AddModelError("strMatricula", "Insira uma matricula correta!");
                    return View(u);
                }
                if (u.strSenha.Length < 1)
                {
                    ModelState.AddModelError("strSenha", "Insira uma senha!");
                    return View(u);
                }
                if (u.strEmail.Length < 1)
                {
                    ModelState.AddModelError("strEmail", "Insira um e-mail para cadastro!");
                    return View(u);
                }
                if (u.strNome.Length < 1)
                {
                    ModelState.AddModelError("strNome", "Insira um Nome para cadastro!");
                    return View(u);
                }

                if (ValidaEmail(u.strEmail) == false)
                {
                    ModelState.AddModelError("strEmail", "Insira um e-mail válido!");
                    return View(u);
                }

                string idG = Session["usuID"].ToString();
                int id;
                if (int.TryParse(idG, out id))
                if (id == null)
                {
                    ModelState.AddModelError("strMatricula", "Não foi possivel carregar os dados!");
                    return View(u);
                }


                var usu = db.tbUsuario.FirstOrDefault(ua => ua.idUsuario == id);
                usu.strMatricula = u.strMatricula;
                usu.strEmail = u.strEmail;
                usu.strNome = u.strNome;
                usu.strSenha = u.strSenha;
                usu.dtCadastro = DateTime.Now;

                db.Entry(usu).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                Session["nome"] = u.strNome.ToString();

                return RedirectToAction("Index", "Home");

            }
            catch (Exception a)
            {
                ModelState.AddModelError("strMatricula", "Erro no cadastro!");
                return View(u);
            }

        }

        [HttpGet]
        public ActionResult GeraToken()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GeraToken(Models.Usuario u)
        {
            try
            {
                if (u.strMatricula.Length != 7)
                {
                    ModelState.AddModelError("strMatricula", "Insira uma matricula correta!");
                    return View(u);
                }

                using (BdJunto bd = new BdJunto())
                {
                    var v = bd.tbUsuario.Where(a => a.strMatricula.Equals(u.strMatricula)).FirstOrDefault();
                    if (v != null)
                    {
                        Random nuAle = new Random();

                        v.strToken = nuAle.Next(100000, 999999).ToString();
                        db.Entry(v).State = System.Data.EntityState.Modified;
                        db.SaveChanges();

                        Session["matToken"] = v.strMatricula.ToString();
                        return RedirectToAction("EntraToken", "Home");
                    }
                }

                ModelState.AddModelError("strMatricula", "Matricula incorreta!");
                return View(u);
            }
            catch (Exception a)
            {
                ModelState.AddModelError("strMatricula", "Erro!");
                return View();
            }

        }


        [HttpGet]
        public ActionResult EntraToken()
        {
            Usuario u = new Usuario();

            string mat = Session["matToken"].ToString();

            u.strMatricula = mat;
            return View(u);
        }
        [HttpPost]
        public ActionResult EntraToken(Models.Usuario u)
        {
            try
            {
                if (u.strMatricula.Length != 7)
                {
                    ModelState.AddModelError("strMatricula", "Insira uma matricula correta!");
                    return View(u);
                }
                if (u.strToken.Length < 1)
                {
                    ModelState.AddModelError("strToken", "Insira o Token para login!");
                    return View(u);
                }

                using (BdJunto bd = new BdJunto())
                {
                    var v = bd.tbUsuario.Where(a => a.strMatricula.Equals(u.strMatricula) && a.strToken.Equals(u.strToken)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["usuID"] = v.idUsuario.ToString();
                        Session["nome"] = v.strNome.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("strMatricula", "Matricula ou Token incorreto!");
                return View(u);
            }
            catch (Exception a)
            {
                ModelState.AddModelError("strMatricula", "Erro!");
                return View();
            }

        }
        private bool ValidaEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
    
}