const API_BASE_URL = "http://localhost:5001";

const loginForm = document.getElementById("loginForm");

if (loginForm) {
    loginForm.addEventListener("submit", async function (event) {
        event.preventDefault();

        const button = loginForm.querySelector(".login-btn");

        const identifier = document
            .getElementById("identifier")
            .value
            .trim();

        const password = document
            .getElementById("password")
            .value;

        if (!identifier || !password) {
            alert("Please enter your login information.");
            return;
        }

        const originalText = button.textContent;

        button.disabled = true;
        button.textContent = "Signing In...";

        try {
            const response = await fetch(
                `${API_BASE_URL}/api/auth/login`,
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        identifier: identifier,
                        password: password
                    })
                }
            );

            const result = await response.json();

            console.log("Complete login response:", result);

            if (response.ok && result.success) {
                // Save student information for the dashboard
                localStorage.setItem(
                    "unimateStudent",
                    JSON.stringify(result.student)
                );

                alert(result.message || "Login successful!");

                // Open dashboard
                window.location.href = "dashboard.html";
            } else {
                alert(
                    result.message ||
                    "Login failed. Please check your credentials."
                );
            }

        } catch (error) {
            console.error("Login error:", error);

            alert(
                "Could not connect to the server. Make sure the C# backend is running."
            );
        } finally {
            button.disabled = false;
            button.textContent = originalText;
        }
    });
}