namespace Noryx.API.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }

        public ICollection<Conta> Contas { get; private set; } = new List<Conta>();

        public ICollection<UsuarioRole> Roles { get; private set; } = new List<UsuarioRole>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

        protected Usuario() { }

        public Usuario(string nome, string email, string senhaHash)
        {
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public void AdicionarRole(string role)
        {
            if (!Roles.Any(r => r.Role == role))
                Roles.Add(new UsuarioRole { Usuario = this, Role = role });
        }

        public void RemoverRole(string role)
        {
            var usuarioRole = Roles.FirstOrDefault(r => r.Role == role);
            if (usuarioRole != null)
                Roles.Remove(usuarioRole);
        }

        public void Atualizar()
        {
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
            Atualizar();
        }

        public void Ativar()
        {
            Ativo = true;
            Atualizar();
        }

        public void AtualizarSenha(string novaSenhaHash)
        {
            SenhaHash = novaSenhaHash;
            Atualizar();
        }

    }
}
