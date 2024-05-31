using Xunit;
using Moq;
using System;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Infrastructure.Authentication;

public class JwtTokenGeneratorTests
{
    private readonly JwtSettings _jwtSettings;
    private readonly Mock<IOptions<JwtSettings>> _mockOptions;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGeneratorTests()
    {
        _jwtSettings = new JwtSettings
        {
            Secret = "YourSecretKeyShouldBeLongEnoughAKJskjkgfjkljdjflsjlfjdsljjfdlkjfs",
            Issuer = "YourIssuer",
            Audience = "YourAudience",
            ExpiryMinutes = 60
        };

        _mockOptions = new Mock<IOptions<JwtSettings>>();
        _mockOptions.Setup(o => o.Value).Returns(_jwtSettings);

        _jwtTokenGenerator = new JwtTokenGenerator(_mockOptions.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        Assert.NotNull(jsonToken);
        Assert.Equal(_jwtSettings.Issuer, jsonToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, jsonToken.Audiences.First());

        var claims = jsonToken.Claims.ToList();
        Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId.ToString());
        Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == firstName);
        Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == lastName);
        Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.Jti);
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectExpiry()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var firstName = "Jane";
        var lastName = "Smith";
        var expectedExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        // Act
        var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        Assert.NotNull(jsonToken);
        var tokenExpiry = jsonToken.ValidTo;
        Assert.True(tokenExpiry > DateTime.UtcNow);
        Assert.True((tokenExpiry - expectedExpiry).TotalSeconds < 5); // Allow a small time difference
    }

    [Fact]
    public void GenerateToken_InvalidSecret_ShouldFailValidation()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var firstName = "Invalid";
        var lastName = "User";
        var invalidSecretKey = "InvalidSecretKey"; // Use an incorrect secret key here
        var handler = new JwtSecurityTokenHandler();

        var validValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
        };

        var invalidValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(invalidSecretKey))
        };

        // Act
        var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);

        // Assert valid token works
        var principal = handler.ValidateToken(token, validValidationParameters, out _);
        Assert.NotNull(principal); // This ensures the token was generated correctly

        // Assert invalid token fails with expected exception
        var exception = Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() => handler.ValidateToken(token, invalidValidationParameters, out _));

        // Optionally check the exception message if needed
        Assert.Contains("Signature validation failed", exception.Message);
    }


}
