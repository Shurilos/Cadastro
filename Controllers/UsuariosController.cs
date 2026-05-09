using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private void ValidarSenha(string senha, string campo = "Senha")
        {
            if (senha.Length < 8)
            {
                ModelState.AddModelError(campo, "A senha deve ter no mínimo 8 caracteres.");
                return;
            }
            if (!Regex.IsMatch(senha, @"[A-Z]"))
                ModelState.AddModelError(campo, "A senha deve conter ao menos uma letra maiúscula.");
            else if (!Regex.IsMatch(senha, @"[a-z]"))
                ModelState.AddModelError(campo, "A senha deve conter ao menos uma letra minúscula.");
            else if (!Regex.IsMatch(senha, @"[0-9]"))
                ModelState.AddModelError(campo, "A senha deve conter ao menos um número.");
            else if (!Regex.IsMatch(senha, @"[^a-zA-Z0-9]"))
                ModelState.AddModelError(campo, "A senha deve conter ao menos um caractere especial (ex: @, #, !).");
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(u => u.Cpf == usuario.Cpf))
                    ModelState.AddModelError("Cpf", "Este CPF já está cadastrado.");

                if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

                if (!string.IsNullOrEmpty(usuario.Telefone) && _context.Usuarios.Any(u => u.Telefone == usuario.Telefone))
                    ModelState.AddModelError("Telefone", "Este telefone já está cadastrado.");

                ValidarSenha(usuario.Senha);

                if (!ModelState.IsValid)
                    return View(usuario);

                usuario.Perfil = "Cliente";
                usuario.DataCadastro = DateTime.Now;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(u => u.Cpf == usuario.Cpf && u.Id != id))
                    ModelState.AddModelError("Cpf", "Este CPF já está cadastrado.");

                if (_context.Usuarios.Any(u => u.Email == usuario.Email && u.Id != id))
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

                if (!string.IsNullOrEmpty(usuario.Telefone) && _context.Usuarios.Any(u => u.Telefone == usuario.Telefone && u.Id != id))
                    ModelState.AddModelError("Telefone", "Este telefone já está cadastrado.");

                ValidarSenha(usuario.Senha);

                if (!ModelState.IsValid)
                    return View(usuario);

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Usuário atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.Id == usuario.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null) _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Usuário removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
