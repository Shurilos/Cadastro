using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly AppDbContext _context;

        public EmpresasController(AppDbContext context)
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

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresas.ToListAsync());
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                if (_context.Empresas.Any(e => e.Cnpj == empresa.Cnpj))
                    ModelState.AddModelError("Cnpj", "Este CNPJ já está cadastrado.");

                if (_context.Empresas.Any(e => e.Email == empresa.Email))
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

                if (!string.IsNullOrEmpty(empresa.Telefone) && _context.Empresas.Any(e => e.Telefone == empresa.Telefone))
                    ModelState.AddModelError("Telefone", "Este telefone já está cadastrado.");

                ValidarSenha(empresa.Senha);

                if (!ModelState.IsValid)
                    return View(empresa);

                empresa.DataCadastro = DateTime.Now;
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Empresa cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empresa empresa)
        {
            if (id != empresa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (_context.Empresas.Any(e => e.Cnpj == empresa.Cnpj && e.Id != id))
                    ModelState.AddModelError("Cnpj", "Este CNPJ já está cadastrado.");

                if (_context.Empresas.Any(e => e.Email == empresa.Email && e.Id != id))
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

                if (!string.IsNullOrEmpty(empresa.Telefone) && _context.Empresas.Any(e => e.Telefone == empresa.Telefone && e.Id != id))
                    ModelState.AddModelError("Telefone", "Este telefone já está cadastrado.");

                ValidarSenha(empresa.Senha);

                if (!ModelState.IsValid)
                    return View(empresa);

                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Empresa atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Empresas.Any(e => e.Id == empresa.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null) _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Empresa removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
