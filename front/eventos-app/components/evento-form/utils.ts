import type { Evento, EventoErrors, Zona } from "./types";

export const initialEvento: Evento = {
  nombre: "",
  fecha: "",
  lugar: "",
  zonas: [
    {
      nombre: "",
      precio: 0,
      capacidad: 0,
    },
  ],
};

export function toDateInput(value: string) {
  return value.slice(0, 10);
}

export function toJsonDate(value: string) {
  if (!value) {
    return "";
  }

  return value.length === 10 ? `${value}T00:00:00` : value;
}

export function getEmptyErrors(zonasLength: number): EventoErrors {
  return {
    nombre: undefined,
    fecha: undefined,
    lugar: undefined,
    zonas: Array.from({ length: zonasLength }, () => ({
      nombre: undefined,
      precio: undefined,
      capacidad: undefined,
    })),
  };
}

export function validateNombre(value: string) {
  if (!value.trim()) {
    return "El nombre es obligatorio.";
  }

  return undefined;
}

export function validateFecha(value: string) {
  if (!value) {
    return "La fecha es obligatoria.";
  }

  if (Number.isNaN(Date.parse(value))) {
    return "La fecha debe tener un formato valido.";
  }

  return undefined;
}

export function validateLugar(value: string) {
  if (!value.trim()) {
    return "El lugar es obligatorio.";
  }

  return undefined;
}

export function validateZonaNombre(value: string) {
  if (!value.trim()) {
    return "El nombre de la zona es obligatorio.";
  }

  return undefined;
}

export function validateZonaPrecio(value: number) {
  if (!Number.isFinite(value) || value <= 0) {
    return "El precio debe ser mayor que 0.";
  }

  return undefined;
}

export function validateZonaCapacidad(value: number) {
  if (!Number.isInteger(value) || value <= 0) {
    return "La capacidad debe ser un entero mayor que 0.";
  }

  return undefined;
}

export function validateEvento(evento: Evento): EventoErrors {
  return {
    nombre: validateNombre(evento.nombre),
    fecha: validateFecha(evento.fecha),
    lugar: validateLugar(evento.lugar),
    zonas: evento.zonas.map((zona) => ({
      nombre: validateZonaNombre(zona.nombre),
      precio: validateZonaPrecio(zona.precio),
      capacidad: validateZonaCapacidad(zona.capacidad),
    })),
  };
}

export function hasErrors(errors: EventoErrors) {
  return Boolean(
    errors.nombre ||
      errors.fecha ||
      errors.lugar ||
      errors.zonas.some(
        (zona) => zona.nombre || zona.precio || zona.capacidad,
      ),
  );
}

export function parseNumberInput(value: string) {
  const parsed = Number(value);

  return Number.isFinite(parsed) ? parsed : 0;
}

export function getInputClass(
  hasError: boolean,
  variant: "default" | "white" = "default",
) {
  const baseClass =
    "w-full rounded-2xl border px-4 py-3 text-sm text-zinc-950 outline-none transition focus:bg-white focus:ring-4";
  const surfaceClass = variant === "white" ? "bg-white" : "bg-zinc-50";
  const stateClass = hasError
    ? "border-rose-300 focus:border-rose-500 focus:ring-rose-100"
    : "border-zinc-200 focus:border-emerald-500 focus:ring-emerald-100";

  return `${baseClass} ${surfaceClass} ${stateClass}`;
}

export function createEmptyZona(): Zona {
  return {
    nombre: "",
    precio: 0,
    capacidad: 0,
  };
}

export function createEmptyZonaErrors() {
  return {
    nombre: undefined,
    precio: undefined,
    capacidad: undefined,
  };
}
