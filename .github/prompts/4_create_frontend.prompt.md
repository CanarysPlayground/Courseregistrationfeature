# Frontend Development Prompt: Course Registration System

You are an expert frontend developer tasked with creating a simple, user-friendly web interface for the Course Registration API system. Build a responsive web application that allows users to manage courses, students, instructors, and departments.

## Task: Create Simple Frontend for Course Registration

Build a clean, functional frontend that consumes the Course Registration API endpoints and provides an intuitive user interface for course management operations.

### Backend API Context

The backend API is running and provides the following endpoints:

#### Available API Endpoints:
- **Courses**: `/api/courses` (GET, POST, PUT, DELETE)
- **Students**: `/api/students` (GET, POST, PUT, DELETE)  
- **Instructors**: `/api/instructors` (GET, POST, PUT, DELETE)
- **Departments**: `/api/departments` (GET, POST, PUT, DELETE)
- **Health Check**: `/api/health` (GET)

#### API Base URL:
- **Development**: `http://localhost:5000/api` (offline mode)
- **HTTPS**: `https://localhost:7229/api` (if using HTTPS)

### Frontend Requirements

#### 1. **Technology Stack**
Choose one of the following approaches:
- **Vanilla HTML/CSS/JavaScript** (Recommended for simplicity)
- **React** (Component-based approach)
- **Vue.js** (Progressive framework)
- **Angular** (Full framework)

#### 2. **Core Features to Implement**

##### **Dashboard/Home Page**
- Welcome message and system overview
- Quick statistics (total courses, students, instructors, departments)
- Navigation menu to different sections
- Health check status indicator

##### **Course Management**
- **List Courses**: Display all courses in a table/card layout
- **Add New Course**: Form to create new courses
- **Edit Course**: Update existing course information
- **Delete Course**: Remove courses with confirmation
- **View Course Details**: Detailed course information

##### **Student Management**
- **List Students**: Display all students
- **Add New Student**: Student registration form
- **Edit Student**: Update student information
- **Delete Student**: Remove students
- **View Student Details**: Student profile and enrolled courses

##### **Instructor Management**  
- **List Instructors**: Display all instructors
- **Add New Instructor**: Instructor registration form
- **Edit Instructor**: Update instructor information
- **Delete Instructor**: Remove instructors
- **View Instructor Details**: Instructor profile and assigned courses

##### **Department Management**
- **List Departments**: Display all departments
- **Add New Department**: Department creation form
- **Edit Department**: Update department information
- **Delete Department**: Remove departments

#### 3. **User Interface Design**

##### **Layout Structure**
```
Header: Course Registration System
├── Navigation Menu
│   ├── Dashboard
│   ├── Courses  
│   ├── Students
│   ├── Instructors
│   └── Departments
├── Main Content Area
│   ├── Page Title
│   ├── Action Buttons (Add New, etc.)
│   └── Data Display (Table/Cards)
└── Footer: System Status
```

##### **Design Guidelines**
- **Responsive Design**: Mobile-first approach
- **Clean Layout**: Simple, uncluttered interface
- **Consistent Styling**: Uniform colors, fonts, spacing
- **Intuitive Navigation**: Clear menu structure
- **Loading States**: Show loading indicators during API calls
- **Error Handling**: User-friendly error messages
- **Success Feedback**: Confirmation messages for actions

#### 4. **Form Specifications**

##### **Add Course Form**
```html
<form id="add-course-form">
  <input type="text" name="title" placeholder="Course Title" required>
  <textarea name="description" placeholder="Course Description" required></textarea>
  <input type="number" name="credits" min="1" max="6" placeholder="Credits" required>
  <input type="number" name="maxCapacity" min="1" max="500" placeholder="Max Capacity" required>
  <input type="date" name="startDate" required>
  <input type="date" name="endDate" required>
  <select name="instructorId" required>
    <option value="">Select Instructor</option>
  </select>
  <select name="departmentId" required>
    <option value="">Select Department</option>
  </select>
  <button type="submit">Create Course</button>
</form>
```

##### **Add Student Form**
```html
<form id="add-student-form">
  <input type="text" name="firstName" placeholder="First Name" required>
  <input type="text" name="lastName" placeholder="Last Name" required>
  <input type="email" name="email" placeholder="Email" required>
  <input type="date" name="dateOfBirth" required>
  <input type="date" name="enrollmentDate" required>
  <input type="tel" name="phone" placeholder="Phone Number">
  <textarea name="address" placeholder="Address"></textarea>
  <button type="submit">Add Student</button>
</form>
```

##### **Add Instructor Form**
```html
<form id="add-instructor-form">
  <input type="text" name="firstName" placeholder="First Name" required>
  <input type="text" name="lastName" placeholder="Last Name" required>
  <input type="email" name="email" placeholder="Email" required>
  <input type="text" name="department" placeholder="Department">
  <input type="date" name="hireDate" required>
  <input type="tel" name="phone" placeholder="Phone Number">
  <textarea name="bio" placeholder="Bio"></textarea>
  <button type="submit">Add Instructor</button>
</form>
```

##### **Add Department Form**
```html
<form id="add-department-form">
  <input type="text" name="name" placeholder="Department Name" required>
  <textarea name="description" placeholder="Description"></textarea>
  <input type="text" name="headOfDepartment" placeholder="Head of Department">
  <button type="submit">Add Department</button>
</form>
```

#### 5. **JavaScript API Integration**

##### **API Service Layer**
```javascript
class ApiService {
  constructor(baseUrl = 'http://localhost:5000/api') {
    this.baseUrl = baseUrl;
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers
      },
      ...options
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`);
    }

    return response.json();
  }

  // Courses
  async getCourses() {
    return this.request('/courses');
  }

  async createCourse(courseData) {
    return this.request('/courses', {
      method: 'POST',
      body: JSON.stringify(courseData)
    });
  }

  async updateCourse(id, courseData) {
    return this.request(`/courses/${id}`, {
      method: 'PUT',
      body: JSON.stringify(courseData)
    });
  }

  async deleteCourse(id) {
    return this.request(`/courses/${id}`, {
      method: 'DELETE'
    });
  }

  // Students
  async getStudents() {
    return this.request('/students');
  }

  // Instructors
  async getInstructors() {
    return this.request('/instructors');
  }

  // Departments
  async getDepartments() {
    return this.request('/departments');
  }

  // Health Check
  async getHealth() {
    return this.request('/health');
  }
}
```

#### 6. **Error Handling and User Feedback**

##### **Error Display**
```javascript
function showError(message) {
  const errorDiv = document.createElement('div');
  errorDiv.className = 'error-message';
  errorDiv.textContent = message;
  document.body.appendChild(errorDiv);
  
  setTimeout(() => {
    errorDiv.remove();
  }, 5000);
}

function showSuccess(message) {
  const successDiv = document.createElement('div');
  successDiv.className = 'success-message';
  successDiv.textContent = message;
  document.body.appendChild(successDiv);
  
  setTimeout(() => {
    successDiv.remove();
  }, 3000);
}
```

##### **Loading States**
```javascript
function showLoading(element) {
  element.disabled = true;
  element.textContent = 'Loading...';
}

function hideLoading(element, originalText) {
  element.disabled = false;
  element.textContent = originalText;
}
```

#### 7. **CSS Styling Guidelines**

##### **Color Scheme**
```css
:root {
  --primary-color: #007bff;
  --secondary-color: #6c757d;
  --success-color: #28a745;
  --danger-color: #dc3545;
  --warning-color: #ffc107;
  --light-color: #f8f9fa;
  --dark-color: #343a40;
}
```

##### **Basic Layout**
```css
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background-color: var(--light-color);
  color: var(--dark-color);
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.header {
  background-color: var(--primary-color);
  color: white;
  padding: 1rem 0;
}

.nav-menu {
  display: flex;
  justify-content: space-around;
  background-color: var(--secondary-color);
  padding: 0.5rem 0;
}

.nav-menu a {
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
}

.nav-menu a:hover {
  background-color: var(--primary-color);
}
```

##### **Forms Styling**
```css
.form-container {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  margin: 2rem 0;
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
}

input, select, textarea {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

button {
  background-color: var(--primary-color);
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}

button:hover {
  background-color: #0056b3;
}
```

##### **Table/Card Styling**
```css
.data-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.data-table th,
.data-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.data-table th {
  background-color: var(--primary-color);
  color: white;
}

.card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  margin: 1rem 0;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
```

#### 8. **File Structure**

```
frontend/
├── index.html                 # Main dashboard
├── css/
│   ├── styles.css            # Main stylesheet
│   └── responsive.css        # Mobile responsive styles
├── js/
│   ├── api.js               # API service layer
│   ├── app.js               # Main application logic
│   ├── courses.js           # Course management
│   ├── students.js          # Student management
│   ├── instructors.js       # Instructor management
│   └── departments.js       # Department management
├── pages/
│   ├── courses.html         # Course management page
│   ├── students.html        # Student management page
│   ├── instructors.html     # Instructor management page
│   └── departments.html     # Department management page
└── assets/
    └── images/              # Icons and images
```

#### 9. **Implementation Steps**

1. **Setup Project Structure**: Create folders and basic HTML files
2. **Create Navigation**: Build header and navigation menu
3. **Implement Dashboard**: Create main dashboard with statistics
4. **Build API Service**: Create JavaScript service for API communication
5. **Develop Course Management**: Implement course CRUD operations
6. **Add Student Management**: Build student management interface
7. **Create Instructor Management**: Implement instructor operations
8. **Build Department Management**: Add department functionality
9. **Style the Application**: Apply CSS styling and responsive design
10. **Test Integration**: Test all features with the backend API
11. **Add Error Handling**: Implement comprehensive error handling
12. **Optimize UX**: Add loading states, confirmations, and feedback

#### 10. **Testing Checklist**

- [ ] All forms submit data correctly to API
- [ ] Data displays properly from API responses
- [ ] Edit functionality works for all entities
- [ ] Delete functionality works with confirmation
- [ ] Error messages display for failed operations
- [ ] Success messages show for completed operations
- [ ] Loading states appear during API calls
- [ ] Responsive design works on mobile devices
- [ ] Navigation works between all pages
- [ ] Health check displays API status

#### 11. **Advanced Features (Optional)**

- **Search and Filter**: Add search functionality for lists
- **Pagination**: Implement pagination for large datasets
- **Sorting**: Add column sorting for tables
- **Bulk Operations**: Select and delete multiple items
- **Export Data**: Export lists to CSV/PDF
- **User Authentication**: Add login/logout functionality
- **Real-time Updates**: WebSocket integration for live updates
- **Offline Support**: Service worker for offline functionality

### Success Criteria

The frontend should provide:
✅ **Intuitive User Interface**: Easy to navigate and use
✅ **Full CRUD Operations**: Create, read, update, delete for all entities
✅ **Responsive Design**: Works on desktop, tablet, and mobile
✅ **Error Handling**: Graceful error handling and user feedback
✅ **API Integration**: Seamless communication with backend API
✅ **Modern UI/UX**: Clean, professional appearance
✅ **Performance**: Fast loading and smooth interactions

This frontend will provide a complete interface for the Course Registration system, allowing users to manage all aspects of course administration through a clean, intuitive web interface.
