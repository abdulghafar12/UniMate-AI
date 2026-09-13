const API_BASE_URL = "http://localhost:5001";

document.addEventListener("DOMContentLoaded", async () => {
    const studentData = localStorage.getItem("unimateStudent");

    if (!studentData) {
        window.location.href = "login.html";
        return;
    }

    const student = JSON.parse(studentData);

    // Basic student information
    setText("welcomeName", student.fullName);
    setText("topStudentName", student.fullName);
    setText("profileName", student.fullName);
    setText("profileRegistration", student.registrationNumber);
    setText("profileEmail", student.universityEmail);
    setText("profileProgram", student.program);
    setText("profileDepartment", student.department);
    setText("semesterBadge", `Semester ${student.semester}`);
    setText("cgpaValue", Number(student.cgpa || 0).toFixed(2));

    const avatar = document.getElementById("topAvatar");

    if (avatar) {
        avatar.textContent = getInitials(student.fullName);
    }

    // Load real academic data
    await loadAcademicData(student.id);

    // Mobile sidebar
    setupSidebar();

    // Logout
    setupLogout();
});

async function loadAcademicData(studentId) {
    try {
        const response = await fetch(
            `${API_BASE_URL}/api/academic/student/${studentId}`
        );

        if (!response.ok) {
            throw new Error("Unable to load academic data.");
        }

        const data = await response.json();

        if (!data.success) {
            throw new Error(data.message || "Academic data not found.");
        }

        console.log("Academic data loaded:", data);

        updateAcademicSummary(data);
        renderSemesterList(data.semesters);
        renderCompletedCourses(data.semesters);
        renderRemainingCourses(data.curriculum, data.semesters);

    } catch (error) {
        console.error("Academic data error:", error);
        showMessage(
            "academicError",
            "Academic data could not be loaded. Please restart the backend."
        );
    }
}

function updateAcademicSummary(data) {
    const summary = data.academicSummary;

    if (!summary) {
        return;
    }

    setText(
        "cgpaValue",
        Number(summary.degreeCGPA || 0).toFixed(2)
    );

    setText(
        "academicStatus",
        summary.academicStatus || "Not Available"
    );

    setText(
        "financialStatus",
        summary.financialStatus || "Not Available"
    );

    setText(
        "currentHolds",
        summary.currentHolds ?? 0
    );

    setText(
        "currentFines",
        summary.currentFines ?? 0
    );

    setText(
        "totalOutstanding",
        `Rs. ${Number(summary.totalOutstanding || 0).toLocaleString()}`
    );
}

function renderSemesterList(semesters) {
    const container = document.getElementById("semesterList");

    if (!container) {
        console.warn("semesterList element not found.");
        return;
    }

    container.innerHTML = "";

    if (!semesters || semesters.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                No academic semester records found.
            </div>
        `;
        return;
    }

    semesters.forEach((semester, index) => {
        const semesterId = `semester-${semester.id}`;

        const gpaText =
            semester.gpa === null || semester.gpa === undefined
                ? "Result Awaited"
                : Number(semester.gpa).toFixed(2);

        const cgpaText =
            semester.cgpa === null || semester.cgpa === undefined
                ? "Result Awaited"
                : Number(semester.cgpa).toFixed(2);

        const courseRows = (semester.courses || [])
            .map(course => {
                const retakeBadge = course.isRetake
                    ? `<span class="badge badge-warning">Retake</span>`
                    : "";

                return `
                    <tr>
                        <td>${escapeHtml(course.courseId)}</td>
                        <td>${escapeHtml(course.courseName)}</td>
                        <td>${escapeHtml(course.creditHours)}</td>
                        <td>
                            <strong>${escapeHtml(course.grade)}</strong>
                            ${retakeBadge}
                        </td>
                        <td>
                            ${
                                course.grade === "Result Awaited"
                                    ? `<span class="muted-text">Pending</span>`
                                    : `<span class="success-text">Completed</span>`
                            }
                        </td>
                    </tr>
                `;
            })
            .join("");

        const card = document.createElement("div");

        card.className = "semester-card";

        card.innerHTML = `
            <div class="semester-header">
                <div>
                    <h3>${escapeHtml(semester.session)}</h3>
                    <p>${escapeHtml(semester.semesterName)}</p>
                </div>

                <button
                    class="details-button"
                    type="button"
                    data-target="${semesterId}">
                    Show Details...
                </button>
            </div>

            <div class="semester-summary">
                <div>
                    <span>GPA</span>
                    <strong>${gpaText}</strong>
                </div>

                <div>
                    <span>CGPA</span>
                    <strong>${cgpaText}</strong>
                </div>

                <div>
                    <span>Status</span>
                    <strong>${escapeHtml(semester.status)}</strong>
                </div>
            </div>

            <div id="${semesterId}" class="semester-details hidden">
                <h4>Courses</h4>

                <div class="table-wrapper">
                    <table class="academic-table">
                        <thead>
                            <tr>
                                <th>Course ID</th>
                                <th>Course Name</th>
                                <th>Credit Hours</th>
                                <th>Grade</th>
                                <th>Status</th>
                            </tr>
                        </thead>

                        <tbody>
                            ${courseRows}
                        </tbody>
                    </table>
                </div>

                <h4>Financial Details</h4>

                <div class="financial-grid">
                    <div>
                        <span>Semester Fee</span>
                        <strong>
                            Rs. ${Number(
                                semester.semesterFee || 0
                            ).toLocaleString()}
                        </strong>
                    </div>

                    <div>
                        <span>Rebate / Concession</span>
                        <strong>
                            Rs. ${Number(
                                semester.rebateOrConcession || 0
                            ).toLocaleString()}
                        </strong>
                    </div>

                    <div>
                        <span>Fee Received</span>
                        <strong>
                            Rs. ${Number(
                                semester.feeReceived || 0
                            ).toLocaleString()}
                        </strong>
                    </div>

                    <div>
                        <span>Remaining Fee</span>
                        <strong>
                            Rs. ${Number(
                                semester.feeRemaining || 0
                            ).toLocaleString()}
                        </strong>
                    </div>

                    <div>
                        <span>Total Outstanding</span>
                        <strong>
                            Rs. ${Number(
                                semester.totalOutstanding || 0
                            ).toLocaleString()}
                        </strong>
                    </div>
                </div>

                ${
                    semester.additionalFeeComments
                        ? `<p class="small-note">
                            <strong>Additional Fee:</strong>
                            ${escapeHtml(
                                semester.additionalFeeComments
                            )}
                           </p>`
                        : ""
                }

                ${
                    semester.rebateComments
                        ? `<p class="small-note">
                            <strong>Rebate:</strong>
                            ${escapeHtml(semester.rebateComments)}
                           </p>`
                        : ""
                }

                ${
                    semester.paymentComments
                        ? `<p class="small-note">
                            <strong>Payment:</strong>
                            ${escapeHtml(semester.paymentComments)}
                           </p>`
                        : ""
                }
            </div>
        `;

        container.appendChild(card);
    });

    document.querySelectorAll(".details-button").forEach(button => {
        button.addEventListener("click", () => {
            const targetId = button.dataset.target;
            const details = document.getElementById(targetId);

            if (!details) {
                return;
            }

            const isHidden = details.classList.contains("hidden");

            details.classList.toggle("hidden", !isHidden);

            button.textContent = isHidden
                ? "Hide Details..."
                : "Show Details...";
        });
    });
}


function renderCompletedCourses(semesters) {
    const container = document.getElementById("completedCourses");

    if (!container) {
        return;
    }

    const completedCourses = [];

    semesters.forEach(semester => {
        (semester.courses || []).forEach(course => {
            if (
                course.resultStatus === "Completed" &&
                course.grade !== "D" &&
                course.grade !== "F"
            ) {
                completedCourses.push({
                    ...course,
                    session: semester.session
                });
            }
        });
    });

    if (completedCourses.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                No completed courses found.
            </div>
        `;
        return;
    }

    const coursesHTML = completedCourses
        .map(course => `
            <div class="course-item">
                <div>
                    <strong>${escapeHtml(course.courseName)}</strong>
                    <p>
                        ${escapeHtml(course.courseId)}
                        • ${escapeHtml(course.session)}
                    </p>
                </div>

                <span class="course-grade">
                    ${escapeHtml(course.grade)}
                </span>
            </div>
        `)
        .join("");

    container.innerHTML = `
        <button
            class="details-button course-details-button"
            type="button"
            data-target="completedCoursesDetails">
            Show Details...
        </button>

        <div
            id="completedCoursesDetails"
            class="course-details-list hidden">

            ${coursesHTML}

        </div>
    `;

    const button = container.querySelector(".course-details-button");
    const details = document.getElementById("completedCoursesDetails");

    button.addEventListener("click", () => {
        const isHidden = details.classList.contains("hidden");

        details.classList.toggle("hidden", !isHidden);

        button.textContent = isHidden
            ? "Hide Details..."
            : "Show Details...";
    });
}


function renderRemainingCourses(curriculum, semesters) {
    const container = document.getElementById("remainingCourses");

    if (!container) {
        return;
    }

    const completedCourseIds = new Set();

    semesters.forEach(semester => {
        (semester.courses || []).forEach(course => {
            const passed =
                course.grade !== "D" &&
                course.grade !== "F" &&
                course.grade !== "Result Awaited";

            if (passed) {
                completedCourseIds.add(course.courseId);
            }
        });
    });

    const remainingCourses = curriculum.filter(course => {
        return !completedCourseIds.has(course.courseId);
    });

    if (remainingCourses.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                All curriculum courses are completed.
            </div>
        `;
        return;
    }

    const coursesHTML = remainingCourses
        .map(course => `
            <div class="course-item">
                <div>
                    <strong>${escapeHtml(course.courseName)}</strong>
                    <p>
                        ${escapeHtml(course.courseId)}
                        • Semester ${escapeHtml(course.semesterNumber)}
                    </p>
                </div>

                <span class="course-credit">
                    ${escapeHtml(course.creditHours)}
                </span>
            </div>
        `)
        .join("");

    container.innerHTML = `
        <button
            class="details-button course-details-button"
            type="button"
            data-target="remainingCoursesDetails">
            Show Details...
        </button>

        <div
            id="remainingCoursesDetails"
            class="course-details-list hidden">

            ${coursesHTML}

        </div>
    `;

    const button = container.querySelector(".course-details-button");
    const details = document.getElementById("remainingCoursesDetails");

    button.addEventListener("click", () => {
        const isHidden = details.classList.contains("hidden");

        details.classList.toggle("hidden", !isHidden);

        button.textContent = isHidden
            ? "Hide Details..."
            : "Show Details...";
    });
}


function setupSidebar() {
    const menuButton = document.getElementById("menuToggle");
    const sidebar = document.querySelector(".sidebar");

    if (!menuButton || !sidebar) {
        return;
    }

    menuButton.addEventListener("click", () => {
        sidebar.classList.toggle("active");
    });
}

function setupLogout() {
    const logoutButton = document.getElementById("logoutButton");

    if (!logoutButton) {
        return;
    }

    logoutButton.addEventListener("click", () => {
        localStorage.removeItem("unimateStudent");
        window.location.href = "login.html";
    });
}

function setText(elementId, value) {
    const element = document.getElementById(elementId);

    if (element) {
        element.textContent = value;
    }
}

function getInitials(name) {
    return name
        .split(" ")
        .filter(Boolean)
        .slice(0, 2)
        .map(word => word[0].toUpperCase())
        .join("");
}

function showMessage(elementId, message) {
    const element = document.getElementById(elementId);

    if (element) {
        element.textContent = message;
        element.classList.remove("hidden");
    }
}

function escapeHtml(value) {
    return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}