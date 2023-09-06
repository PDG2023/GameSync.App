using Microsoft.AspNetCore.Identity;
#pragma warning disable IDE0090 // Utiliser 'new(...)'

#pragma warning disable CS8765 // La nullabilité de type du paramètre ne correspond pas au membre substitué (probablement en raison des attributs de nullabilité).
namespace GameSync.Api;
/// <inheritdoc />
/// <summary>
/// Unshamefully copied from : https://stackoverflow.com/a/50729694
/// Service to enable localization (french) for application facing identity errors.
/// </summary>
public class FrenchIdentityErrorDescriber : IdentityErrorDescriber
{
    /// <inheritdoc />
    public override IdentityError DefaultError() => new IdentityError { Code = nameof(DefaultError), Description = "Une erreur inconnue est survenue." };

    /// <inheritdoc />
    public override IdentityError ConcurrencyFailure() => new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Erreur de concurrence simultanée optimiste, l'objet a été modifié." };

    /// <inheritdoc />
    public override IdentityError PasswordMismatch() => new IdentityError { Code = nameof(PasswordMismatch), Description = "Mot de passe incorrect." };

    /// <inheritdoc />
    public override IdentityError InvalidToken() => new IdentityError { Code = nameof(InvalidToken), Description = "Jeton invalide." };

    /// <inheritdoc />
    public override IdentityError LoginAlreadyAssociated() => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Un utilisateur avec ce nom de compte existe déjà." };

    /// <inheritdoc />
    public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = nameof(InvalidUserName), Description = $"Le nom de compte '{userName}' est invalide. Seuls les lettres et chiffres sont autorisés." };

    /// <inheritdoc />
    public override IdentityError InvalidEmail(string email) => new IdentityError { Code = nameof(InvalidEmail), Description = $"L'email '{email}' est invalide." };

    /// <inheritdoc />
    public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = nameof(DuplicateUserName), Description = $"Le nom de compte '{userName}' est déjà utilisé." };

    /// <inheritdoc />
    public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = nameof(DuplicateEmail), Description = $"L'email '{email} est déjà utilisée." };

    /// <inheritdoc />
    public override IdentityError InvalidRoleName(string role) => new IdentityError { Code = nameof(InvalidRoleName), Description = $"Le nom du rôle '{role}' est invalide." };

    /// <inheritdoc />
    public override IdentityError DuplicateRoleName(string role) => new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Le nom du rôle '{role}' est déjà utilisé." };

    /// <inheritdoc />
    public override IdentityError UserAlreadyHasPassword() => new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "L'utilisateur a déjà un mot de passe." };

    /// <inheritdoc />
    public override IdentityError UserLockoutNotEnabled() => new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Le verouillage n'est pas activé pour cet utilisateur." };

    /// <inheritdoc />
    public override IdentityError UserAlreadyInRole(string role) => new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"L'utilisateur a déjà le rôle '{role}'." };

    /// <inheritdoc />
    public override IdentityError UserNotInRole(string role) => new IdentityError { Code = nameof(UserNotInRole), Description = $"L'utilisateur n'a pas le rôle '{role}'." };

    /// <inheritdoc />
    public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = nameof(PasswordTooShort), Description = $"Le mot de passe doit contenir au moins {length} caractères." };

    /// <inheritdoc />
    public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Le mot de passe doit contenir au moins un caractère non alpha-numérique." };

    /// <inheritdoc />
    public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Le mot de passe doit contenir au moins un chiffre ('0'-'9')." };

    /// <inheritdoc />
    public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Le mot de passe doit contenir au moins un charactère minuscule ('a'-'z')." };

    /// <inheritdoc />
    public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Le mot de passe doit contenir au moins un charactère majuscule ('A'-'Z')." };
}
#pragma warning restore CS8765 // La nullabilité de type du paramètre ne correspond pas au membre substitué (probablement en raison des attributs de nullabilité).
#pragma warning restore IDE0090 // Utiliser 'new(...)'
