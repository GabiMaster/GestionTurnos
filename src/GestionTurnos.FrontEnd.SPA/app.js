function spaApp() {
    return {
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
                    body: JSON.stringify(this.loginData)
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.jwt = data.token;
                    localStorage.setItem('jwt', this.jwt);
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
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    this.servicios = await resp.json();
                } else {
                    this.servicios = [];
                }
            } catch {
                this.servicios = [];
            }
        },
        async loadTurnos() {
            try {
                const resp = await fetch('/api/turnos', {
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
            }
            this.loading = false;
        },
        async cancelarTurno(id) {
            try {
                const resp = await fetch(`/api/turnos/${id}`, {
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
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
        }
    }
}
