// File: wwwroot/js/booking-script.js

class BookingFormHandler {
    constructor(formId) {
        this.form = document.getElementById(formId);
        this.initialize();
    }

    initialize() {
        if (!this.form) return;

        // Set minimum date to today
        const dateInput = this.form.querySelector('#Booking_EventDate');
        if (dateInput) {
            const today = new Date().toISOString().split('T')[0];
            dateInput.min = today;
            dateInput.value = today;
        }

        // Set default time to 6:00 PM
        const timeInput = this.form.querySelector('#Booking_EventTime');
        if (timeInput && !timeInput.value) {
            timeInput.value = '18:00';
        }

        // Initialize form validation
        this.setupValidation();

        // Setup service selection
        this.setupServiceSelection();

        // Setup real-time validation
        this.setupRealTimeValidation();

        // Setup form submission
        this.setupFormSubmission();
    }

    setupValidation() {
        // Add Bootstrap validation classes
        this.form.addEventListener('submit', (e) => {
            if (!this.form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }

            this.form.classList.add('was-validated');
        }, false);
    }

    setupServiceSelection() {
        const serviceCheckboxes = this.form.querySelectorAll('.service-checkbox');
        serviceCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', () => {
                this.updateSelectedServices();
            });
        });
    }

    updateSelectedServices() {
        const selectedServices = [];
        const checkboxes = this.form.querySelectorAll('.service-checkbox:checked');

        checkboxes.forEach(checkbox => {
            selectedServices.push(checkbox.value);
        });

        // Update hidden field or display
        const displayElement = this.form.querySelector('#selectedServicesDisplay');
        if (displayElement) {
            if (selectedServices.length > 0) {
                displayElement.textContent = `Selected: ${selectedServices.join(', ')}`;
                displayElement.classList.remove('d-none');
            } else {
                displayElement.classList.add('d-none');
            }
        }

        return selectedServices;
    }

    setupRealTimeValidation() {
        // Real-time email validation
        const emailInput = this.form.querySelector('#Booking_Email');
        if (emailInput) {
            emailInput.addEventListener('blur', () => {
                this.validateEmail(emailInput);
            });
        }

        // Real-time phone validation
        const phoneInput = this.form.querySelector('#Booking_Phone');
        if (phoneInput) {
            phoneInput.addEventListener('blur', () => {
                this.validatePhone(phoneInput);
            });
        }

        // Date validation
        const dateInput = this.form.querySelector('#Booking_EventDate');
        if (dateInput) {
            dateInput.addEventListener('change', () => {
                this.validateDate(dateInput);
            });
        }
    }

    validateEmail(input) {
        const email = input.value;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (email && !emailRegex.test(email)) {
            this.showError(input, 'Please enter a valid email address');
            return false;
        }

        this.clearError(input);
        return true;
    }

    validatePhone(input) {
        const phone = input.value.replace(/\D/g, ''); // Remove non-digits

        if (phone && phone.length < 10) {
            this.showError(input, 'Please enter a valid phone number (at least 10 digits)');
            return false;
        }

        // Format phone number
        if (phone.length >= 10) {
            input.value = this.formatPhoneNumber(phone);
        }

        this.clearError(input);
        return true;
    }

    formatPhoneNumber(phone) {
        // Format as +63 XXX XXX XXXX
        const cleaned = phone.replace(/\D/g, '');
        const match = cleaned.match(/^(\d{2})(\d{3})(\d{3})(\d{4})$/);
        if (match) {
            return `+${match[1]} ${match[2]} ${match[3]} ${match[4]}`;
        }
        return phone;
    }

    validateDate(input) {
        const selectedDate = new Date(input.value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (selectedDate < today) {
            this.showError(input, 'Event date must be today or in the future');
            return false;
        }

        this.clearError(input);
        return true;
    }

    showError(input, message) {
        const formGroup = input.closest('.form-group') || input.closest('.mb-3');
        if (!formGroup) return;

        // Remove existing error
        this.clearError(input);

        // Add error class
        input.classList.add('is-invalid');

        // Create error message
        const errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        errorDiv.textContent = message;

        formGroup.appendChild(errorDiv);
    }

    clearError(input) {
        input.classList.remove('is-invalid');

        const formGroup = input.closest('.form-group') || input.closest('.mb-3');
        if (formGroup) {
            const existingError = formGroup.querySelector('.invalid-feedback');
            if (existingError) {
                existingError.remove();
            }
        }
    }

    setupFormSubmission() {
        this.form.addEventListener('submit', async (e) => {
            e.preventDefault();

            if (!this.validateForm()) {
                return;
            }

            // Show loading state
            this.setLoadingState(true);

            try {
                // Prepare form data
                const formData = new FormData(this.form);

                // Add selected services
                const selectedServices = this.updateSelectedServices();
                formData.append('SelectedServices', selectedServices.join(','));

                // Submit form
                const response = await fetch(this.form.action, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                });

                if (response.ok) {
                    const result = await response.text();

                    // Check if it's a redirect
                    if (response.redirected) {
                        window.location.href = response.url;
                    } else {
                        // Show success message
                        this.showSuccessMessage();
                        this.form.reset();
                        this.form.classList.remove('was-validated');
                    }
                } else {
                    throw new Error('Form submission failed');
                }
            } catch (error) {
                console.error('Submission error:', error);
                this.showErrorMessage('An error occurred. Please try again.');
            } finally {
                this.setLoadingState(false);
            }
        });
    }

    validateForm() {
        let isValid = true;

        // Check required fields
        const requiredFields = this.form.querySelectorAll('[required]');
        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                this.showError(field, 'This field is required');
                isValid = false;
            }
        });

        // Validate email
        const emailField = this.form.querySelector('#Booking_Email');
        if (emailField && emailField.value) {
            if (!this.validateEmail(emailField)) {
                isValid = false;
            }
        }

        // Validate date
        const dateField = this.form.querySelector('#Booking_EventDate');
        if (dateField && dateField.value) {
            if (!this.validateDate(dateField)) {
                isValid = false;
            }
        }

        return isValid;
    }

    setLoadingState(isLoading) {
        const submitButton = this.form.querySelector('button[type="submit"]');
        if (!submitButton) return;

        if (isLoading) {
            submitButton.disabled = true;
            submitButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Processing...';
        } else {
            submitButton.disabled = false;
            submitButton.innerHTML = 'Submit Booking Request';
        }
    }

    showSuccessMessage() {
        // Create success alert
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-success alert-dismissible fade show mt-3';
        alertDiv.innerHTML = `
            <strong>Success!</strong> Your booking request has been submitted. We'll contact you within 24 hours.
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        // Insert after form
        this.form.parentNode.insertBefore(alertDiv, this.form.nextSibling);

        // Auto-remove after 10 seconds
        setTimeout(() => {
            alertDiv.remove();
        }, 10000);
    }

    showErrorMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger alert-dismissible fade show mt-3';
        alertDiv.innerHTML = `
            <strong>Error!</strong> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        this.form.parentNode.insertBefore(alertDiv, this.form.nextSibling);
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    const bookingFormHandler = new BookingFormHandler('bookingForm');

    // Additional form enhancements
    enhanceFormExperience();
});

function enhanceFormExperience() {
    // Auto-format phone number
    const phoneInput = document.querySelector('#Booking_Phone');
    if (phoneInput) {
        phoneInput.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 0) {
                value = '+63 ' + value.substring(2);
            }
            e.target.value = value.substring(0, 19); // Limit length
        });
    }

    // Calculate estimated cost based on selections
    const calculateEstimate = () => {
        const duration = document.querySelector('#Booking_DurationHours')?.value || 0;
        const guests = document.querySelector('#Booking_EstimatedGuests')?.value || 0;
        const services = document.querySelectorAll('.service-checkbox:checked').length;

        if (duration > 0 && guests > 0) {
            let baseCost = 5000;
            let serviceCost = services * 1000;
            let guestCost = Math.max(0, guests - 50) * 50; // Additional cost over 50 guests
            let durationCost = duration * 500;

            let estimate = baseCost + serviceCost + guestCost + durationCost;

            // Display estimate
            const estimateElement = document.getElementById('costEstimate');
            if (estimateElement) {
                estimateElement.textContent = `Estimated Cost: ₱${estimate.toLocaleString()}`;
                estimateElement.classList.remove('d-none');
            }
        }
    };

    // Add event listeners for cost calculation
    ['#Booking_DurationHours', '#Booking_EstimatedGuests'].forEach(selector => {
        const element = document.querySelector(selector);
        if (element) {
            element.addEventListener('change', calculateEstimate);
        }
    });

    // Service checkboxes
    document.querySelectorAll('.service-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', calculateEstimate);
    });
}