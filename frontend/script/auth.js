const API_BASE_URL = "http://localhost:5001";

const registerForm = document.getElementById("registerForm");

if (registerForm) {

registerForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const button = registerForm.querySelector(".register-btn");

    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;

    // Frontend password check
    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return;
    }

    if (password.length < 8) {
        alert("Password must be at least 8 characters.");
        return;
    }

    // Convert semester from string to number
    const semester = Number(
        document.getElementById("semester").value
    );

    // Data exactly matching RegisterRequest.cs
    const registerData = {
        fullName: document.getElementById("fullName").value.trim(),
        registrationNumber: document.getElementById("registrationNumber").value.trim(),
        universityEmail: document.getElementById("universityEmail").value.trim(),
        personalEmail: document.getElementById("personalEmail").value.trim() || null,
        phoneNumber: document.getElementById("phoneNumber").value.trim() || null,
        password: password,
        confirmPassword: confirmPassword,
        program: document.getElementById("program").value,
        department: document.getElementById("department").value,
        semester: semester
    };

    // Loading state
    const originalText = button.textContent;
    button.disabled = true;
    button.textContent = "Creating Account...";

    try {

        const response = await fetch(
            `${API_BASE_URL}/api/auth/register`,
            {
                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify(registerData)
            }
        );

        const result = await response.json();

        if (response.ok && result.success) {

            alert(
                result.message ||
                "Registration successful!"
            );

            registerForm.reset();

            // Later we can redirect to login
            // window.location.href = "login.html";

        } else {

            alert(
                result.message ||
                "Registration failed. Please check your information."
            );
        }

    } catch (error) {

        console.error("Registration error:", error);

        alert(
            "Could not connect to the server. " +
            "Make sure the C# backend is running."
        );

    } finally {

        button.disabled = false;
        button.textContent = originalText;

    }

});

}
