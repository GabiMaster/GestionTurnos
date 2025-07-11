function spaApp() {
    return {
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
                    body: JSON.stringify(this.loginData)
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.jwt = data.token;
                    localStorage.setItem('jwt', this.jwt);
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
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    this.servicios = await resp.json();
                } else {
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
            }
            this.loading = false;
        },
        async cancelarTurno(id) {
            if (!confirm('¿Cancelar este turno?')) return;
            this.loading = true;
            try {
                const resp = await fetch(this.apiBase + `/turnos/${id}`, {
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
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
        }
    }
}
