function spaApp() {
    return {
<<<<<<< HEAD
        page: 'login',
        jwt: localStorage.getItem('jwt') || null,
        loginData: { email: '', password: '' },
        loginMessage: '',
        registerData: { nombre: '', email: '', password: '' },
        registerMessage: '',
        servicios: [],
        turnos: [],
        selectedServicio: null,
        nuevoTurno: { fechaHora: '' },
        turnoMensaje: '',
        qrUrl: '',
        loading: false,
        goTo(p) {
            this.page = p;
            this.loginMessage = '';
            this.registerMessage = '';
            this.turnoMensaje = '';
            this.qrUrl = '';
            this.selectedServicio = null;
            this.nuevoTurno.fechaHora = '';
        },
        async init() {
            if (this.jwt) {
                this.page = 'main';
                await this.loadServicios();
                await this.loadTurnos();
            }
        },
        async login() {
            this.loginMessage = '';
            this.loginData.email = this.loginData.email.trim().toLowerCase();
            let apiUrl = '/api/auth/login';
            if (window.location.hostname === 'localhost') {
                apiUrl = 'https://localhost:7298/api/auth/login';
            }
            try {
                const resp = await fetch(apiUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
=======
        jwt: localStorage.getItem('jwt') || null,
        loginData: { email: '', password: '' },
        registerData: { usuario: '', nombre: '', apellido: '', email: '', password: '' },
        servicios: [],
        turnos: [],
        nuevoTurno: { servicioId: '', fechaHora: '' },
        mensaje: '',
        mensajeRegistro: '',
        qrUrl: '',
        loading: false,
        showRegister: false,
        apiBase: 'https://localhost:7087/api', // Cambia si tu API está en otro host/puerto
        async init() {
            if (this.jwt) {
                await this.cargarServicios();
                await this.cargarTurnos();
            }
        },
        async login() {
            this.mensaje = '';
            this.loading = true;
            try {
                const resp = await fetch(this.apiBase + '/auth/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=UTF-8' },
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
                    body: JSON.stringify(this.loginData)
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.jwt = data.token;
                    localStorage.setItem('jwt', this.jwt);
<<<<<<< HEAD
                    this.page = 'main';
                    await this.loadServicios();
                    await this.loadTurnos();
                } else {
                    let msg = 'Credenciales incorrectas.';
                    try {
                        const err = await resp.json();
                        if (err.mensaje) msg = err.mensaje;
                    } catch {}
                    this.loginMessage = msg;
                }
            } catch {
                this.loginMessage = 'Error de conexión.';
            }
        },
        async register() {
            this.registerMessage = '';
            const payload = {
                Usuario: this.registerData.nombre.trim(),
                Nombre: this.registerData.nombre.trim(),
                Apellido: '',
                Email: this.registerData.email.trim().toLowerCase(),
                Password: this.registerData.password
            };
            try {
                const resp = await fetch('/api/auth/register', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });
                if (resp.ok) {
                    this.registerMessage = 'Registro exitoso. Ahora puedes iniciar sesión.';
                } else {
                    this.registerMessage = 'No se pudo registrar.';
                }
            } catch {
                this.registerMessage = 'Error de conexión.';
            }
        },
        async loadServicios() {
            try {
                const resp = await fetch('/api/servicios', {
=======
                    this.mensaje = '';
                    await this.cargarServicios();
                    await this.cargarTurnos();
                } else {
                    const data = await resp.json().catch(() => ({}));
                    this.mensaje = data.mensaje || 'Credenciales incorrectas';
                }
            } catch {
                this.mensaje = 'Error de red al iniciar sesión';
            }
            this.loading = false;
        },
        async register() {
            this.mensajeRegistro = '';
            this.loading = true;
            try {
                const resp = await fetch(this.apiBase + '/auth/register', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=UTF-8' },
                    body: JSON.stringify({
                        usuario: this.registerData.usuario,
                        nombre: this.registerData.nombre,
                        apellido: this.registerData.apellido,
                        email: this.registerData.email,
                        password: this.registerData.password,
                        rol: 0 // Cliente
                    })
                });
                if (resp.ok) {
                    this.mensajeRegistro = 'Usuario registrado con éxito. Ahora puedes iniciar sesión.';
                    this.showRegister = false;
                } else {
                    const err = await resp.text();
                    this.mensajeRegistro = 'Error al registrar: ' + err;
                }
            } catch {
                this.mensajeRegistro = 'Error de red al registrar.';
            }
            this.loading = false;
        },
        logout() {
            this.jwt = null;
            localStorage.removeItem('jwt');
            this.servicios = [];
            this.turnos = [];
            this.nuevoTurno = { servicioId: '', fechaHora: '' };
            this.qrUrl = '';
            this.mensaje = '';
        },
        async cargarServicios() {
            this.servicios = [];
            this.mensaje = '';
            try {
                const resp = await fetch(this.apiBase + '/servicios', {
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    this.servicios = await resp.json();
                } else {
<<<<<<< HEAD
                    this.servicios = [];
                }
            } catch {
                this.servicios = [];
            }
        },
        async loadTurnos() {
            try {
                const resp = await fetch('/api/turnos', {
=======
                    this.mensaje = 'No se pudieron cargar los servicios.';
                }
            } catch {
                this.mensaje = 'Error de red al consultar servicios.';
            }
        },
        async cargarTurnos() {
            this.turnos = [];
            try {
                const resp = await fetch(this.apiBase + '/turnos/mis-turnos', {
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    this.turnos = await resp.json();
                } else {
                    this.turnos = [];
                }
            } catch {
                this.turnos = [];
            }
        },
<<<<<<< HEAD
        selectServicio(id) {
            this.selectedServicio = id;
            this.nuevoTurno = { fechaHora: '' };
            this.turnoMensaje = '';
            this.qrUrl = '';
        },
        async agendarTurno() {
            this.loading = true;
            this.turnoMensaje = '';
            this.qrUrl = '';
            try {
                const resp = await fetch('/api/turnos', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify({ servicioId: this.selectedServicio, fechaHora: this.nuevoTurno.fechaHora })
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.turnoMensaje = 'Turno agendado correctamente.';
                    this.qrUrl = data.tokenQR;
                    await this.loadTurnos();
                } else {
                    this.turnoMensaje = 'No se pudo agendar el turno.';
                }
            } catch {
                this.turnoMensaje = 'Error de conexión.';
=======
        async agendarTurno() {
            this.mensaje = '';
            this.qrUrl = '';
            this.loading = true;
            try {
                const resp = await fetch(this.apiBase + '/turnos', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json; charset=UTF-8',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify({
                        servicioId: parseInt(this.nuevoTurno.servicioId),
                        fechaHora: this.nuevoTurno.fechaHora
                    })
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.mensaje = 'Turno agendado correctamente.';
                    this.qrUrl = data.tokenQR || data.TokenQR || '';
                    await this.cargarTurnos();
                } else {
                    const err = await resp.text();
                    this.mensaje = 'Error al agendar turno: ' + err;
                }
            } catch {
                this.mensaje = 'Error de red al agendar turno.';
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
            }
            this.loading = false;
        },
        async cancelarTurno(id) {
<<<<<<< HEAD
            try {
                const resp = await fetch(`/api/turnos/${id}`, {
=======
            if (!confirm('¿Cancelar este turno?')) return;
            this.loading = true;
            try {
                const resp = await fetch(this.apiBase + `/turnos/${id}`, {
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
<<<<<<< HEAD
                    await this.loadTurnos();
                }
            } catch {}
        },
        logout() {
            this.jwt = null;
            localStorage.removeItem('jwt');
            this.page = 'login';
            this.servicios = [];
            this.turnos = [];
=======
                    this.mensaje = 'Turno cancelado.';
                    await this.cargarTurnos();
                } else {
                    this.mensaje = 'No se pudo cancelar el turno.';
                }
            } catch {
                this.mensaje = 'Error de red al cancelar turno.';
            }
            this.loading = false;
        },
        formatFechaHora(fecha) {
            if (!fecha) return '';
            try {
                const d = new Date(fecha);
                return d.toLocaleString('es-AR', { dateStyle: 'short', timeStyle: 'short' });
            } catch {
                return fecha;
            }
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4
        }
    }
}
