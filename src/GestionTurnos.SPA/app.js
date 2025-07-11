function spaApp() {
    return {
        page: 'login',
        loginData: { email: '', password: '' },
        loginMessage: '',
        registerData: { nombre: '', email: '', password: '' },
        registerMessage: '',
        servicios: [],
        profesionales: [],
        profesionalesFiltrados: [],
        turnos: [],
        selectedServicio: null,
        nuevoTurno: { servicioId: '', profesionalId: '', fechaHora: '' },
        turnoMensaje: '',
        qrUrl: '',
        loading: false,
        jwt: localStorage.getItem('jwt') || null,
        rol: localStorage.getItem('rol') || null,
        passwordData: { actual: '', nueva: '' },
        passwordMessage: '',
        nuevoServicio: { nombre: '', duracionMinutos: '' },
        servicioEdit: { id: '', nombre: '', duracionMinutos: '' },
        servicioMensaje: '',
        nuevoProfesional: { nombreCompleto: '', especialidad: '', serviciosIds: [] },
        profesionalEdit: { id: '', nombreCompleto: '', especialidad: '', serviciosIds: [] },
        profesionalMensaje: '',
        recuperarEmail: '',
        recuperarMensaje: '',
        init() {
            if (this.jwt) {
                this.page = 'main';
                this.fetchServicios();
                this.fetchProfesionales();
                this.fetchTurnos();
            } else {
                this.page = 'login';
            }
        },
        goTo(p) {
            this.page = p;
            this.loginMessage = '';
            this.registerMessage = '';
            this.servicioMensaje = '';
            this.profesionalMensaje = '';
        },
        async login() {
            this.loginMessage = '';
            try {
                const resp = await fetch('https://localhost:7087/api/auth/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email: this.loginData.email, password: this.loginData.password })
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.jwt = data.token;
                    this.rol = data.rol || null;
                    localStorage.setItem('jwt', this.jwt);
                    localStorage.setItem('rol', this.rol);
                    this.page = 'main';
                    this.fetchServicios();
                    this.fetchProfesionales();
                    this.fetchTurnos();
                } else {
                    const err = await resp.json();
                    this.loginMessage = err.mensaje || 'Credenciales incorrectas';
                }
            } catch {
                this.loginMessage = 'Error de red.';
            }
        },
        async register() {
            this.registerMessage = '';
            try {
                const resp = await fetch('https://localhost:7087/api/auth/register', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        nombre: this.registerData.nombre,
                        email: this.registerData.email,
                        password: this.registerData.password
                    })
                });
                if (resp.ok) {
                    this.registerMessage = 'Registro exitoso. Ahora puedes iniciar sesión.';
                    setTimeout(() => this.goTo('login'), 1500);
                } else {
                    const err = await resp.json();
                    this.registerMessage = err.mensaje || 'No se pudo registrar.';
                }
            } catch {
                this.registerMessage = 'Error de red.';
            }
        },
        async fetchServicios() {
            if (!this.jwt) return;
            try {
                const resp = await fetch('https://localhost:7087/api/servicios', {
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
        async fetchProfesionales() {
            if (!this.jwt) return;
            try {
                const resp = await fetch('https://localhost:7087/api/profesionales', {
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.profesionales = data.map(p => ({
                        ...p,
                        serviciosIds: p.serviciosIds || p.servicios?.map(s => s.id) || []
                    }));
                } else {
                    this.profesionales = [];
                }
            } catch {
                this.profesionales = [];
            }
        },
        onServicioChange() {
            const sid = parseInt(this.nuevoTurno.servicioId);
            this.profesionalesFiltrados = this.profesionales.filter(p => Array.isArray(p.serviciosIds) && p.serviciosIds.includes(sid));
            this.nuevoTurno.profesionalId = '';
        },
        async fetchTurnos() {
            if (!this.jwt) return;
            try {
                const resp = await fetch('https://localhost:7087/api/turnos/mis-turnos', {
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    const turnos = await resp.json();
                    this.turnos = turnos.map(t => ({
                        ...t,
                        servicioNombre: t.servicio?.nombre || ''
                    }));
                } else {
                    this.turnos = [];
                }
            } catch {
                this.turnos = [];
            }
        },
        selectServicio(id) {
            this.selectedServicio = this.servicios.find(s => s.id === id);
            this.nuevoTurno = { servicioId: id, profesionalId: '', fechaHora: '' };
            this.onServicioChange();
            this.turnoMensaje = '';
            this.qrUrl = '';
        },
        async agendarTurno() {
            if (!this.jwt || !this.nuevoTurno.servicioId || !this.nuevoTurno.profesionalId || !this.nuevoTurno.fechaHora) return;
            this.loading = true;
            this.turnoMensaje = '';
            this.qrUrl = '';
            try {
                const resp = await fetch('https://localhost:7087/api/turnos/agendar', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify({
                        servicioId: this.nuevoTurno.servicioId,
                        profesionalId: this.nuevoTurno.profesionalId,
                        fechaHora: this.nuevoTurno.fechaHora
                    })
                });
                if (resp.ok) {
                    const data = await resp.json();
                    this.turnoMensaje = 'Turno agendado correctamente.';
                    this.qrUrl = data.tokenQR || data.TokenQR || '';
                    await this.fetchTurnos();
                } else {
                    const err = await resp.text();
                    this.turnoMensaje = 'Error al agendar turno: ' + err;
                }
            } catch {
                this.turnoMensaje = 'Error de red al agendar turno.';
            }
            this.loading = false;
        },
        async cancelarTurno(id) {
            if (!this.jwt) return;
            try {
                const resp = await fetch(`https://localhost:7087/api/turnos/${id}`, {
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    await this.fetchTurnos();
                }
            } catch {}
        },
        logout() {
            localStorage.removeItem('jwt');
            localStorage.removeItem('rol');
            this.jwt = null;
            this.rol = null;
            this.page = 'login';
            this.loginData = { email: '', password: '' };
            this.registerData = { nombre: '', email: '', password: '' };
            this.servicios = [];
            this.profesionales = [];
            this.profesionalesFiltrados = [];
            this.turnos = [];
            this.selectedServicio = null;
            this.nuevoTurno = { servicioId: '', profesionalId: '', fechaHora: '' };
            this.turnoMensaje = '';
            this.qrUrl = '';
        },
        async cambiarPassword() {
            this.passwordMessage = '';
            if (!this.jwt) return;
            try {
                const resp = await fetch('https://localhost:7087/api/auth/cambiar-password', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify({
                        actual: this.passwordData.actual,
                        nueva: this.passwordData.nueva
                    })
                });
                if (resp.ok) {
                    this.passwordMessage = 'Contraseña cambiada correctamente.';
                    this.passwordData = { actual: '', nueva: '' };
                } else {
                    const err = await resp.json();
                    this.passwordMessage = err.mensaje || 'No se pudo cambiar la contraseña.';
                }
            } catch {
                this.passwordMessage = 'Error de red.';
            }
        },
        editarServicio(s) {
            this.servicioEdit = { ...s };
            this.goTo('editar-servicio');
        },
        async guardarNuevoServicio() {
            this.servicioMensaje = '';
            try {
                const resp = await fetch('https://localhost:7087/api/servicios', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify(this.nuevoServicio)
                });
                if (resp.ok) {
                    this.servicioMensaje = 'Servicio creado correctamente.';
                    this.nuevoServicio = { nombre: '', duracionMinutos: '' };
                    await this.fetchServicios();
                    this.goTo('admin-servicios');
                } else {
                    const err = await resp.text();
                    this.servicioMensaje = 'Error: ' + err;
                }
            } catch {
                this.servicioMensaje = 'Error de red.';
            }
        },
        async guardarEdicionServicio() {
            this.servicioMensaje = '';
            try {
                const resp = await fetch(`https://localhost:7087/api/servicios/${this.servicioEdit.id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify(this.servicioEdit)
                });
                if (resp.ok) {
                    this.servicioMensaje = 'Servicio actualizado correctamente.';
                    await this.fetchServicios();
                    this.goTo('admin-servicios');
                } else {
                    const err = await resp.text();
                    this.servicioMensaje = 'Error: ' + err;
                }
            } catch {
                this.servicioMensaje = 'Error de red.';
            }
        },
        async eliminarServicio(id) {
            if (!confirm('¿Eliminar servicio?')) return;
            try {
                const resp = await fetch(`https://localhost:7087/api/servicios/${id}`, {
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    await this.fetchServicios();
                }
            } catch {}
        },
        editarProfesional(p) {
            this.profesionalEdit = { ...p };
            this.goTo('editar-profesional');
        },
        async guardarNuevoProfesional() {
            this.profesionalMensaje = '';
            try {
                const resp = await fetch('https://localhost:7087/api/profesionales', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify(this.nuevoProfesional)
                });
                if (resp.ok) {
                    this.profesionalMensaje = 'Profesional creado correctamente.';
                    this.nuevoProfesional = { nombreCompleto: '', especialidad: '', serviciosIds: [] };
                    await this.fetchProfesionales();
                    this.goTo('admin-profesionales');
                } else {
                    const err = await resp.text();
                    this.profesionalMensaje = 'Error: ' + err;
                }
            } catch {
                this.profesionalMensaje = 'Error de red.';
            }
        },
        async guardarEdicionProfesional() {
            this.profesionalMensaje = '';
            try {
                const resp = await fetch(`https://localhost:7087/api/profesionales/${this.profesionalEdit.id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.jwt
                    },
                    body: JSON.stringify(this.profesionalEdit)
                });
                if (resp.ok) {
                    this.profesionalMensaje = 'Profesional actualizado correctamente.';
                    await this.fetchProfesionales();
                    this.goTo('admin-profesionales');
                } else {
                    const err = await resp.text();
                    this.profesionalMensaje = 'Error: ' + err;
                }
            } catch {
                this.profesionalMensaje = 'Error de red.';
            }
        },
        async eliminarProfesional(id) {
            if (!confirm('¿Eliminar profesional?')) return;
            try {
                const resp = await fetch(`https://localhost:7087/api/profesionales/${id}`, {
                    method: 'DELETE',
                    headers: { 'Authorization': 'Bearer ' + this.jwt }
                });
                if (resp.ok) {
                    await this.fetchProfesionales();
                }
            } catch {}
        },
        async enviarRecuperarPassword() {
            this.recuperarMensaje = '';
            if (!this.recuperarEmail) return;
            try {
                const resp = await fetch('https://localhost:7087/api/auth/recuperar-password', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email: this.recuperarEmail })
                });
                if (resp.ok) {
                    this.recuperarMensaje = 'Si el email existe, se ha enviado un enlace de recuperación.';
                } else {
                    this.recuperarMensaje = 'No se pudo procesar la solicitud.';
                }
            } catch {
                this.recuperarMensaje = 'Error de red.';
            }
        },
    }
}
