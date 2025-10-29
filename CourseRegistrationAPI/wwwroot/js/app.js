// API Base URL
const API_BASE_URL = window.location.origin + '/api';

// DOM Elements
let modal;
let registerButton;
let closeButton;
let cancelButton;
let registrationForm;
let roleSelect;
let studentSection;
let instructorSection;
let studentIdSelect;
let instructorIdSelect;
let errorMessage;
let successMessage;

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initializeElements();
    attachEventListeners();
    loadSystemInfo();
    loadStudents();
    loadInstructors();
});

// Initialize DOM elements
function initializeElements() {
    modal = document.getElementById('registrationModal');
    registerButton = document.getElementById('registerButton');
    closeButton = document.querySelector('.close');
    cancelButton = document.getElementById('cancelButton');
    registrationForm = document.getElementById('registrationForm');
    roleSelect = document.getElementById('role');
    studentSection = document.getElementById('studentSection');
    instructorSection = document.getElementById('instructorSection');
    studentIdSelect = document.getElementById('studentId');
    instructorIdSelect = document.getElementById('instructorId');
    errorMessage = document.getElementById('errorMessage');
    successMessage = document.getElementById('successMessage');
}

// Attach event listeners
function attachEventListeners() {
    // Registration button click - opens modal
    registerButton.addEventListener('click', openRegistrationModal);
    
    // Close modal buttons
    closeButton.addEventListener('click', closeRegistrationModal);
    cancelButton.addEventListener('click', closeRegistrationModal);
    
    // Close modal when clicking outside
    window.addEventListener('click', function(event) {
        if (event.target === modal) {
            closeRegistrationModal();
        }
    });
    
    // Role selection change
    roleSelect.addEventListener('change', handleRoleChange);
    
    // Form submission
    registrationForm.addEventListener('submit', handleRegistrationSubmit);
}

// Open registration modal
function openRegistrationModal() {
    modal.style.display = 'block';
    clearMessages();
    registrationForm.reset();
    handleRoleChange(); // Reset role-specific sections
}

// Close registration modal
function closeRegistrationModal() {
    modal.style.display = 'none';
    clearMessages();
    registrationForm.reset();
}

// Handle role selection change
function handleRoleChange() {
    const selectedRole = roleSelect.value;
    
    // Hide both sections by default
    studentSection.style.display = 'none';
    instructorSection.style.display = 'none';
    
    // Remove required attribute from both
    studentIdSelect.removeAttribute('required');
    instructorIdSelect.removeAttribute('required');
    
    // Show and require appropriate section based on role
    if (selectedRole === '0') { // Student
        studentSection.style.display = 'block';
        studentIdSelect.setAttribute('required', 'required');
    } else if (selectedRole === '1') { // Instructor
        instructorSection.style.display = 'block';
        instructorIdSelect.setAttribute('required', 'required');
    }
}

// Handle registration form submission
async function handleRegistrationSubmit(event) {
    event.preventDefault();
    clearMessages();
    
    const formData = new FormData(registrationForm);
    const userData = {
        username: formData.get('username'),
        email: formData.get('email'),
        password: formData.get('password'),
        firstName: formData.get('firstName'),
        lastName: formData.get('lastName'),
        phone: formData.get('phone') || null,
        role: parseInt(formData.get('role'))
    };
    
    // Add studentId or instructorId based on role
    if (userData.role === 0 && formData.get('studentId')) {
        userData.studentId = parseInt(formData.get('studentId'));
    } else if (userData.role === 1 && formData.get('instructorId')) {
        userData.instructorId = parseInt(formData.get('instructorId'));
    }
    
    try {
        setFormLoading(true);
        const response = await fetch(`${API_BASE_URL}/users`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showSuccess(`Registration successful! Welcome, ${result.username}!`);
            setTimeout(() => {
                closeRegistrationModal();
            }, 2000);
        } else {
            const error = await response.text();
            showError(error || 'Registration failed. Please try again.');
        }
    } catch (error) {
        showError('An error occurred during registration. Please try again.');
        console.error('Registration error:', error);
    } finally {
        setFormLoading(false);
    }
}

// Load students for dropdown
async function loadStudents() {
    try {
        const response = await fetch(`${API_BASE_URL}/students`);
        if (response.ok) {
            const students = await response.json();
            populateStudentSelect(students);
        }
    } catch (error) {
        console.error('Error loading students:', error);
    }
}

// Populate student dropdown
function populateStudentSelect(students) {
    studentIdSelect.innerHTML = '<option value="">Select Student Profile</option>';
    students.forEach(student => {
        const option = document.createElement('option');
        option.value = student.id;
        option.textContent = `${student.firstName} ${student.lastName} (${student.email})`;
        studentIdSelect.appendChild(option);
    });
}

// Load instructors for dropdown
async function loadInstructors() {
    try {
        const response = await fetch(`${API_BASE_URL}/instructors`);
        if (response.ok) {
            const instructors = await response.json();
            populateInstructorSelect(instructors);
        }
    } catch (error) {
        console.error('Error loading instructors:', error);
    }
}

// Populate instructor dropdown
function populateInstructorSelect(instructors) {
    instructorIdSelect.innerHTML = '<option value="">Select Instructor Profile</option>';
    instructors.forEach(instructor => {
        const option = document.createElement('option');
        option.value = instructor.id;
        option.textContent = `${instructor.firstName} ${instructor.lastName} (${instructor.email})`;
        instructorIdSelect.appendChild(option);
    });
}

// Load system information
async function loadSystemInfo() {
    try {
        const response = await fetch(`${API_BASE_URL}/info`);
        if (response.ok) {
            const info = await response.json();
            displaySystemInfo(info);
        } else {
            throw new Error('Failed to load system info');
        }
    } catch (error) {
        displaySystemInfo({ error: 'Unable to load system information' });
        console.error('Error loading system info:', error);
    }
}

// Display system information
function displaySystemInfo(info) {
    const infoBox = document.getElementById('systemInfo');
    
    // Clear previous content
    infoBox.innerHTML = '';
    
    if (info.error) {
        const errorPara = document.createElement('p');
        errorPara.style.color = 'var(--danger-color)';
        errorPara.textContent = info.error;
        infoBox.appendChild(errorPara);
        return;
    }
    
    // Create elements safely using textContent
    const createInfoParagraph = (label, value) => {
        const p = document.createElement('p');
        const strong = document.createElement('strong');
        strong.textContent = label + ': ';
        p.appendChild(strong);
        // Handle null/undefined values gracefully
        const displayValue = (value !== null && value !== undefined) ? String(value) : 'N/A';
        p.appendChild(document.createTextNode(displayValue));
        return p;
    };
    
    infoBox.appendChild(createInfoParagraph('Title', info.title));
    infoBox.appendChild(createInfoParagraph('Version', info.version));
    infoBox.appendChild(createInfoParagraph('Description', info.description));
    infoBox.appendChild(createInfoParagraph('Database', info.database));
    infoBox.appendChild(createInfoParagraph('Sample Data', info.sampleData));
}

// Show error message
function showError(message) {
    errorMessage.textContent = message;
    errorMessage.classList.add('show');
    successMessage.classList.remove('show');
}

// Show success message
function showSuccess(message) {
    successMessage.textContent = message;
    successMessage.classList.add('show');
    errorMessage.classList.remove('show');
}

// Clear messages
function clearMessages() {
    errorMessage.classList.remove('show');
    successMessage.classList.remove('show');
    errorMessage.textContent = '';
    successMessage.textContent = '';
}

// Set form loading state
function setFormLoading(isLoading) {
    const submitButton = registrationForm.querySelector('button[type="submit"]');
    const inputs = registrationForm.querySelectorAll('input, select, button');
    
    if (isLoading) {
        submitButton.classList.add('loading');
        submitButton.textContent = 'Registering';
        inputs.forEach(input => input.disabled = true);
    } else {
        submitButton.classList.remove('loading');
        submitButton.textContent = 'Register';
        inputs.forEach(input => input.disabled = false);
    }
}
