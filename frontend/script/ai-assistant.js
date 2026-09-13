/* =========================================================
   UNIMATE AI ASSISTANT — SCRIPT
   Sections:
   1. Tool configuration
   2. Conversation storage (per tool, multiple conversations)
   3. Boot
   4. Tool selection
   5. Rendering (messages, welcome card, history list)
   6. Sending messages + simulated AI reply
   7. Scrolling
   8. Input behavior (auto-grow, enter-to-send, disable-when-empty)
   9. Sidebar controls (desktop collapse/handle, mobile drawers)
========================================================= */

/* 0. Real viewport height (fixes input hidden behind mobile keyboard /
      address bar resize glitches on phones and tablets) */

function setRealViewportHeight() {
    const height = window.visualViewport ? window.visualViewport.height : window.innerHeight;
    document.documentElement.style.setProperty("--app-height", `${height}px`);
}

setRealViewportHeight();
window.addEventListener("resize", setRealViewportHeight);
window.addEventListener("orientationchange", setRealViewportHeight);
if (window.visualViewport) {
    window.visualViewport.addEventListener("resize", setRealViewportHeight);
}

/* 1. Tool configuration */

const toolSettings = {

    chat: {
        theme: "theme-chat",
        label: "AI CHAT",
        title: "Your Personal AI Academic Assistant",
        description: "Ask questions about your courses, CGPA, university life and academic progress.",
        welcomeTitle: "Hello! How can I help you today?",
        welcomeDescription: "Ask UniMate AI about your academic record, courses, CGPA or university-related questions.",
        placeholder: "Ask UniMate AI...",
        hint: "Your personalized AI chat will be connected next.",
        suggestions: [
            "How can I improve my CGPA?",
            "Explain my remaining courses.",
            "What should I study first?"
        ],
        simulatedReply: "Thanks for the question! Once UniMate AI is fully connected, I'll give you a detailed, personalized answer based on your academic record."
    },

    viva: {
        theme: "theme-viva",
        label: "AI VIVA COACH",
        title: "Practice Your Viva With AI",
        description: "Prepare for viva questions, technical interviews and subject discussions.",
        welcomeTitle: "Ready for your mock viva?",
        welcomeDescription: "Choose a subject or topic and practice answering questions like a real examiner.",
        placeholder: "Enter a subject or topic for viva practice...",
        hint: "Viva questions and answer evaluation will be connected next.",
        suggestions: [
            "Start a DSA viva.",
            "Ask me an OOP question.",
            "Take a difficult AI viva."
        ],
        simulatedReply: "Good choice. Once the viva engine is connected, I'll ask you real examiner-style questions on this topic and score your answers."
    },

    "course-map": {
        theme: "theme-course-map",
        label: "COURSE DEPENDENCY MAP",
        title: "Understand Your Course Dependencies",
        description: "Explore prerequisites and understand which subjects support your future courses.",
        welcomeTitle: "Explore your academic course map",
        welcomeDescription: "UniMate will show relationships between foundation courses and advanced subjects.",
        placeholder: "Ask about course prerequisites...",
        hint: "Your university course dependency map will be connected next.",
        suggestions: [
            "What is the prerequisite of DSA?",
            "Explain my DBMS course path.",
            "Which foundation subjects should I revise?"
        ],
        simulatedReply: "Once connected to your course catalog, I'll map out exactly which subjects lead into this one and what to revise first."
    },

    deadlines: {
        theme: "theme-deadlines",
        label: "DEADLINE ASSISTANT",
        title: "Never Miss an Important University Deadline",
        description: "Track fees, assignments, registrations, scholarships and other important dates.",
        welcomeTitle: "Let's check your important deadlines",
        welcomeDescription: "UniMate will help you understand upcoming, urgent and overdue university tasks.",
        placeholder: "Ask about fees, deadlines or notifications...",
        hint: "Real deadline alerts and notifications will be connected next.",
        suggestions: [
            "What deadlines are coming?",
            "What happens if I pay my fee late?",
            "Remind me about my assignments."
        ],
        simulatedReply: "Once connected to the university calendar, I'll pull your real deadlines and flag anything urgent right here."
    },

    documents: {
        theme: "theme-documents",
        label: "AI DOCUMENT ASSISTANT",
        title: "Understand Forms, Applications and Documents",
        description: "Upload or explain a document and receive guidance about missing information and next steps.",
        welcomeTitle: "Need help with a university document?",
        welcomeDescription: "You will be able to analyze forms, scholarship documents, applications and university notices.",
        placeholder: "Ask about a document or application...",
        hint: "Document upload and AI analysis will be connected next.",
        suggestions: [
            "Write a retake application.",
            "Explain a scholarship form.",
            "Write an email to my HOD."
        ],
        simulatedReply: "Once document analysis is connected, I'll read your form and tell you exactly what's missing and how to fill it in."
    },

    career: {
        theme: "theme-career",
        label: "AI CAREER ADVISOR",
        title: "Discover Your Best Career Path",
        description: "Get career recommendations based on your degree, skills, interests and academic progress.",
        welcomeTitle: "Let's plan your career direction",
        welcomeDescription: "UniMate will identify suitable career paths, skill gaps, projects and certifications.",
        placeholder: "Ask about your career path...",
        hint: "Personalized career recommendations will be connected next.",
        suggestions: [
            "Which career suits my skills?",
            "How can I become an AI developer?",
            "What skills should I learn next?"
        ],
        simulatedReply: "Once connected to your academic profile, I'll suggest career paths that fit your skills and interests, plus what to learn next."
    },

    internships: {
        theme: "theme-internships",
        label: "AI INTERNSHIP MATCHER",
        title: "Find Internships That Match Your Skills",
        description: "Match your degree, skills, interests and location with suitable internship opportunities.",
        welcomeTitle: "Find your next internship",
        welcomeDescription: "UniMate will compare your profile with employer requirements and explain skill gaps.",
        placeholder: "Find internships based on my skills...",
        hint: "Internship matching will be connected next.",
        suggestions: [
            "Find frontend internships for me.",
            "What skills do backend internships require?",
            "Am I ready for an AI internship?"
        ],
        simulatedReply: "Once internship matching is connected, I'll compare your skills with real listings and show you the best fits."
    },

    roadmap: {
        theme: "theme-roadmap",
        label: "AI ROADMAP GENERATOR",
        title: "Build a Personalized Learning Roadmap",
        description: "Generate a practical roadmap based on your goal, current skills, time and target duration.",
        welcomeTitle: "What do you want to become?",
        welcomeDescription: "Create a step-by-step roadmap for development, AI, automation, internships or any career goal.",
        placeholder: "What roadmap should I generate for you?",
        hint: "Your AI-powered roadmap generator will be connected next.",
        suggestions: [
            "Create a backend developer roadmap.",
            "Generate an AI automation roadmap.",
            "Make a 3-month Python roadmap."
        ],
        simulatedReply: "Once the roadmap generator is connected, I'll build you a week-by-week plan tailored to your goal and timeline."
    },

    journey: {
        theme: "theme-journey",
        label: "MY ACADEMIC JOURNEY",
        title: "Understand Your Academic Journey",
        description: "Analyze your semester performance, CGPA trend, strengths, weaknesses and next priorities.",
        welcomeTitle: "Let's understand your academic journey",
        welcomeDescription: "UniMate will use your academic record to explain your progress and recommend your next steps.",
        placeholder: "Ask about my academic progress...",
        hint: "Your real academic journey analysis will be connected next.",
        suggestions: [
            "Explain my CGPA trend.",
            "What are my weakest subjects?",
            "Create my academic improvement plan."
        ],
        simulatedReply: "Once connected to your transcript, I'll break down your CGPA trend and tell you exactly where to focus next."
    }

};

const TOOL_NAMES = Object.keys(toolSettings);
const API_BASE_URL = (() => {
    const candidates = [
        "http://localhost:5001",
        "http://127.0.0.1:5001",
        "http://0.0.0.0:5001"
    ];

    for (const candidate of candidates) {
        try {
            const url = new URL(candidate);
            if (url.hostname === "localhost" || url.hostname === "127.0.0.1" || url.hostname === "0.0.0.0") {
                return candidate;
            }
        } catch {
            // Ignore invalid URL values.
        }
    }

    return "http://localhost:5001";
})();


/* 2. Conversation storage */

// toolConversations[tool] is an array of { id, title, messages: [{role, text, timestamp}] }
const toolConversations = {};

// activeConversation[tool] points at the conversation object currently shown for that tool.
// It only gets added to toolConversations[tool] once it holds its first message.
const activeConversation = {};

let currentTool = "chat";
TOOL_NAMES.forEach(tool => {
    toolConversations[tool] = [];
    activeConversation[tool] = null;
});

function createConversation() {
    return {
        id: null,
        title: null,
        messages: []
    };
}

function ensureActiveConversation(tool) {
    if (!activeConversation[tool]) {
        activeConversation[tool] = createConversation();
    }
    return activeConversation[tool];
}


/* 3. Boot */

document.addEventListener("DOMContentLoaded", () => {

    loadStudentProfile();
    setupToolButtons();
    setupSendButton();
    setupInputAutoGrow();
    setupNewChatButton();
    setupSidebarControls();

    selectTool("chat");

});


/* 4. Tool selection */

function setupToolButtons() {

    const buttons = document.querySelectorAll(".tool-button");

    buttons.forEach(button => {
        button.addEventListener("click", () => {

            const toolName = button.dataset.tool;

            buttons.forEach(item => item.classList.remove("active"));
            button.classList.add("active");

            selectTool(toolName);
        });
    });
}

function selectTool(toolName) {

    const settings = toolSettings[toolName];

    if (!settings) {
        return;
    }

    currentTool = toolName;

    document.body.className = settings.theme;

    setText("selectedToolLabel", settings.label);
    setText("selectedToolTitle", settings.title);
    setText("selectedToolDescription", settings.description);
    setText("welcomeTitle", settings.welcomeTitle);
    setText("welcomeDescription", settings.welcomeDescription);
    setText("inputHint", settings.hint);
    setText("historyTitle", `${titleCase(settings.label)} History`);

    const input = document.getElementById("aiInput");
    if (input) {
        input.placeholder = settings.placeholder;
    }

    renderSuggestions(settings.suggestions);
    ensureActiveConversation(toolName);
    renderCurrentConversation();
    renderHistory();
    void loadToolConversations(toolName);
}

function getStudentId() {
    try {
        const student = JSON.parse(localStorage.getItem("unimateStudent") || "null");
        return String(student?.id || "demo-student");
    } catch {
        return "demo-student";
    }
}

async function loadToolConversations(tool) {
    try {
        const response = await fetch(
            `${API_BASE_URL}/api/ai/conversations/${encodeURIComponent(tool)}?studentId=${encodeURIComponent(getStudentId())}`
        );

        if (!response.ok) {
            throw new Error("Unable to load conversation history.");
        }

        const summaries = await response.json();
        toolConversations[tool] = summaries.map(conversation => ({
            id: conversation.id,
            title: conversation.title,
            messages: []
        }));

        if (currentTool === tool) {
            const active = activeConversation[tool];
            if (!active || !active.id) {
                activeConversation[tool] = createConversation();
            }
            renderHistory();
        }
    } catch (error) {
        console.error("Conversation history error:", error);
    }
}

function titleCase(label) {
    return label
        .toLowerCase()
        .split(" ")
        .map(word => word.charAt(0).toUpperCase() + word.slice(1))
        .join(" ");
}


/* 5. Rendering */

function renderSuggestions(suggestions) {

    const suggestionList = document.getElementById("suggestionList");

    if (!suggestionList) {
        return;
    }

    suggestionList.innerHTML = suggestions
        .map(suggestion => `
            <button type="button" class="suggestion-button">${escapeHtml(suggestion)}</button>
        `)
        .join("");

    suggestionList.querySelectorAll(".suggestion-button").forEach(button => {
        button.addEventListener("click", () => {
            const input = document.getElementById("aiInput");
            if (!input) {
                return;
            }
            input.value = button.textContent.trim();
            input.focus();
            autoGrowTextarea(input);
            updateSendButtonState();
        });
    });
}

function renderCurrentConversation() {

    const chatMessages = document.getElementById("chatMessages");
    const welcomeCard = document.getElementById("welcomeCard");

    if (!chatMessages) {
        return;
    }

    chatMessages.innerHTML = "";

    const convo = activeConversation[currentTool];
    const messages = convo ? convo.messages : [];

    if (messages.length === 0) {
        if (welcomeCard) welcomeCard.style.display = "";
        scrollChatToBottom();
        return;
    }

    if (welcomeCard) welcomeCard.style.display = "none";

    messages.forEach(message => {
        addMessageToScreen(message.role, message.text);
    });

    scrollChatToBottom();
}

function hideWelcomeCard() {
    const welcomeCard = document.getElementById("welcomeCard");
    if (welcomeCard) welcomeCard.style.display = "none";
}

function addMessageToScreen(role, text) {

    const chatMessages = document.getElementById("chatMessages");

    if (!chatMessages) {
        return;
    }

    const isUser = role === "user";

    const messageElement = document.createElement("div");
    messageElement.className = isUser ? "message user-message" : "message ai-message";

    messageElement.innerHTML = `
        <div class="message-avatar">${isUser ? "You" : "AI"}</div>
        <div class="message-content">
            <strong>${isUser ? "You" : "UniMate AI"}</strong>
            <p>${escapeHtml(text)}</p>
        </div>
    `;

    chatMessages.appendChild(messageElement);
}

function showTypingIndicator() {

    const chatMessages = document.getElementById("chatMessages");
    if (!chatMessages) return;

    const indicator = document.createElement("div");
    indicator.className = "message ai-message typing-indicator";
    indicator.id = "typingIndicator";

    indicator.innerHTML = `
        <div class="message-avatar">AI</div>
        <div class="message-content">
            <div class="typing-dots"><span></span><span></span><span></span></div>
        </div>
    `;

    chatMessages.appendChild(indicator);
    scrollChatToBottom();
}

function removeTypingIndicator() {
    const indicator = document.getElementById("typingIndicator");
    if (indicator) indicator.remove();
}

function renderHistory() {

    const historyList = document.getElementById("historyList");

    if (!historyList) {
        return;
    }

    const conversations = toolConversations[currentTool] || [];
    const activeId = activeConversation[currentTool] ? activeConversation[currentTool].id : null;

    if (conversations.length === 0) {
        historyList.innerHTML = `
            <div class="empty-history">No conversations in this tool yet. Send a message to start one.</div>
        `;
        return;
    }

    historyList.innerHTML = conversations
        .map(convo => `
            <button
                type="button"
                class="history-item${convo.id === activeId ? " active" : ""}"
                data-conversation-id="${convo.id}">
                <strong>${escapeHtml(convo.title || "New conversation")}</strong>
                <small>Today</small>
            </button>
        `)
        .join("");

    historyList.querySelectorAll(".history-item").forEach(item => {
        item.addEventListener("click", () => {
            loadConversation(currentTool, item.dataset.conversationId);
        });
    });
}

async function loadConversation(tool, conversationId) {

    const convo = (toolConversations[tool] || []).find(c => c.id === conversationId);

    if (!convo) {
        return;
    }

    try {
        const response = await fetch(
            `${API_BASE_URL}/api/ai/conversations/${encodeURIComponent(conversationId)}?studentId=${encodeURIComponent(getStudentId())}`
        );

        if (!response.ok) {
            throw new Error("Unable to load this conversation.");
        }

        const savedConversation = await response.json();
        convo.title = savedConversation.title;
        convo.messages = savedConversation.messages.map(message => ({
            role: message.role,
            text: message.content,
            timestamp: Date.parse(message.createdAt) || Date.now()
        }));

        activeConversation[tool] = convo;
        if (currentTool === tool) {
            renderCurrentConversation();
            renderHistory();
        }
    } catch (error) {
        console.error("Conversation load error:", error);
        setText("inputHint", "Could not load this conversation. Please try again.");
    }
}


/* 6. Sending messages to the real backend AI service */

function setupSendButton() {

    const sendButton = document.getElementById("sendButton");
    const input = document.getElementById("aiInput");

    if (!sendButton || !input) {
        return;
    }

    sendButton.addEventListener("click", () => sendCurrentMessage());

    input.addEventListener("keydown", event => {
        if (event.key === "Enter" && !event.shiftKey) {
            event.preventDefault();
            sendCurrentMessage();
        }
    });
}

async function sendCurrentMessage() {

    const input = document.getElementById("aiInput");
    const sendButton = document.getElementById("sendButton");

    if (!input) return;

    const message = input.value.trim();

    if (!message) {
        return;
    }

    sendButton.disabled = true;
    await addUserMessage(message);

    input.value = "";
    autoGrowTextarea(input);
    updateSendButtonState();
    input.focus();
}

async function addUserMessage(text) {

    const tool = currentTool;
    const convo = ensureActiveConversation(tool);
    const isFirstMessage = convo.messages.length === 0;

    convo.messages.push({
        role: "user",
        text: text,
        timestamp: Date.now()
    });

    if (!convo.title) {
        convo.title = text.length > 60 ? `${text.slice(0, 60)}…` : text;
    }

    if (isFirstMessage) {
        toolConversations[tool].unshift(convo);
    }

    hideWelcomeCard();
    addMessageToScreen("user", text);
    scrollChatToBottom();
    renderHistory();

    showTypingIndicator();

    try {
        const request = {
            tool,
            studentId: getStudentId(),
            message: text
        };

        if (convo.id) {
            request.conversationId = convo.id;
        }

        const response = await fetch(`${API_BASE_URL}/api/ai/messages`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(request)
        });

        const result = await response.json().catch(() => null);

        if (!response.ok || !result) {
            throw new Error(result?.message || "UniMate AI could not answer right now.");
        }

        convo.id = result.conversation.id;
        convo.title = result.conversation.title;
        convo.messages = result.conversation.messages.map(message => ({
            role: message.role,
            text: message.content,
            timestamp: Date.parse(message.createdAt) || Date.now()
        }));

        if (currentTool === tool && activeConversation[tool] === convo) {
            renderCurrentConversation();
        }
    } catch (error) {
        console.error("UniMate AI request error:", error);
        const replyText = error.message || "UniMate AI could not answer right now. Please try again.";
        convo.messages.push({ role: "assistant", text: replyText, timestamp: Date.now() });

        if (currentTool === tool && activeConversation[tool] === convo) {
            addMessageToScreen("assistant", replyText);
        }
    } finally {
        removeTypingIndicator();
        renderHistory();
        updateSendButtonState();
        scrollChatToBottom();
    }
}

function setupNewChatButton() {

    const button = document.getElementById("newChatButton");

    if (!button) {
        return;
    }

    button.addEventListener("click", () => {

        const tool = currentTool;
        const convo = activeConversation[tool];

        if (convo && convo.messages.length === 0) {
            // Already on a fresh, empty conversation — nothing to do.
            renderCurrentConversation();
            return;
        }

        activeConversation[tool] = createConversation();
        renderCurrentConversation();
        renderHistory();

        const input = document.getElementById("aiInput");
        if (input) input.focus();
    });
}


/* 7. Scrolling */

function scrollChatToBottom() {

    const chatMessages = document.getElementById("chatMessages");

    if (!chatMessages) {
        return;
    }

    requestAnimationFrame(() => {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    });
}

window.addEventListener("resize", () => {
    scrollChatToBottom();
});


/* 8. Input behavior */

const INPUT_MAX_HEIGHT = 160;

function setupInputAutoGrow() {

    const input = document.getElementById("aiInput");

    if (!input) {
        return;
    }

    input.addEventListener("input", () => {
        autoGrowTextarea(input);
        updateSendButtonState();
    });

    // On phones/tablets, the keyboard opening shrinks the visible viewport —
    // make sure the input and latest message are still on screen when that happens.
    input.addEventListener("focus", () => {
        setTimeout(() => {
            scrollChatToBottom();
            input.scrollIntoView({ block: "end", behavior: "smooth" });
        }, 300);
    });

    updateSendButtonState();
}

function autoGrowTextarea(input) {
    input.style.height = "auto";
    input.style.height = `${Math.min(input.scrollHeight, INPUT_MAX_HEIGHT)}px`;
}

function updateSendButtonState() {

    const input = document.getElementById("aiInput");
    const sendButton = document.getElementById("sendButton");

    if (!input || !sendButton) {
        return;
    }

    sendButton.disabled = input.value.trim().length === 0;
}


/* 9. Sidebar controls */

const DESKTOP_BREAKPOINT = 1024;

function setupSidebarControls() {

    const toolsSidebar = document.getElementById("toolsSidebar");
    const historySidebar = document.getElementById("historySidebar");
    const overlay = document.getElementById("sidebarOverlay");

    const openToolsMobileButton = document.getElementById("openToolsSidebar");
    const openHistoryMobileButton = document.getElementById("openHistorySidebarMobile");

    const closeToolsButton = document.getElementById("closeToolsSidebar");
    const closeHistoryButton = document.getElementById("closeHistorySidebar");

    const openToolsHandle = document.getElementById("openToolsHandle");
    const openHistoryHandle = document.getElementById("openHistoryHandle");

    function isDesktop() {
        return window.innerWidth > DESKTOP_BREAKPOINT;
    }

    function openMobileDrawer(sidebar) {
        if (!sidebar) return;
        sidebar.classList.add("mobile-open");
        overlay?.classList.add("active");
    }

    function closeMobileDrawers() {
        toolsSidebar?.classList.remove("mobile-open");
        historySidebar?.classList.remove("mobile-open");
        overlay?.classList.remove("active");
    }

    openToolsMobileButton?.addEventListener("click", () => openMobileDrawer(toolsSidebar));
    openHistoryMobileButton?.addEventListener("click", () => openMobileDrawer(historySidebar));
    overlay?.addEventListener("click", closeMobileDrawers);

    closeToolsButton?.addEventListener("click", () => {
        if (isDesktop()) {
            toolsSidebar?.classList.add("collapsed");
            document.querySelector(".ai-workspace")?.classList.add("tools-collapsed");
        } else {
            closeMobileDrawers();
        }
    });

    closeHistoryButton?.addEventListener("click", () => {
        if (isDesktop()) {
            historySidebar?.classList.add("collapsed");
            document.querySelector(".ai-workspace")?.classList.add("history-collapsed");
        } else {
            closeMobileDrawers();
        }
    });

    openToolsHandle?.addEventListener("click", () => {
        toolsSidebar?.classList.remove("collapsed");
        document.querySelector(".ai-workspace")?.classList.remove("tools-collapsed");
    });

    openHistoryHandle?.addEventListener("click", () => {
        historySidebar?.classList.remove("collapsed");
        document.querySelector(".ai-workspace")?.classList.remove("history-collapsed");
    });

    window.addEventListener("resize", () => {
        if (isDesktop()) {
            closeMobileDrawers();
        }
    });
}


/* Utilities */

function loadStudentProfile() {

    const studentData = localStorage.getItem("unimateStudent");

    if (!studentData) {
        return;
    }

    try {
        const student = JSON.parse(studentData);
        const avatar = document.getElementById("studentAvatar");

        if (avatar && student.fullName) {
            avatar.textContent = getInitials(student.fullName);
        }
    } catch (error) {
        console.error("Unable to load student profile:", error);
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

function setText(elementId, value) {
    const element = document.getElementById(elementId);
    if (element) {
        element.textContent = value;
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
