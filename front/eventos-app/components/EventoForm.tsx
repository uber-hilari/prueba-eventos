"use client";

import { useState } from "react";

type Zona = {
  nombre: string;
  precio: number;
  capacidad: number;
};

type Evento = {
  nombre: string;
  fecha: string;
  lugar: string;
  zonas: Zona[];
};

type EventoFormProps = {
  title?: string;
  description?: string;
  submitLabel?: string;
};

type ZonaErrors = {
  nombre?: string;
  precio?: string;
  capacidad?: string;
};

type EventoErrors = {
  nombre?: string;
  fecha?: string;
  lugar?: string;
  zonas: ZonaErrors[];
};

type SubmitState = {
  status: "idle" | "success" | "error";
  message?: string;
};

const initialEvento: Evento = {
  nombre: "",
  fecha: "",
  lugar: "",
  zonas: [
    {
      nombre: "",
      precio: 0,
      capacidad: 0,
    }
  ],
};

function toDateInput(value: string) {
  return value.slice(0, 10);
}

function toJsonDate(value: string) {
  if (!value) {
    return "";
  }

  return value.length === 10 ? `${value}T00:00:00` : value;
}

function getEmptyErrors(zonasLength: number): EventoErrors {
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

function validateNombre(value: string) {
  if (!value.trim()) {
    return "El nombre es obligatorio.";
  }

  return undefined;
}

function validateFecha(value: string) {
  if (!value) {
    return "La fecha es obligatoria.";
  }

  if (Number.isNaN(Date.parse(value))) {
    return "La fecha debe tener un formato valido.";
  }

  return undefined;
}

function validateLugar(value: string) {
  if (!value.trim()) {
    return "El lugar es obligatorio.";
  }

  return undefined;
}

function validateZonaNombre(value: string) {
  if (!value.trim()) {
    return "El nombre de la zona es obligatorio.";
  }

  return undefined;
}

function validateZonaPrecio(value: number) {
  if (!Number.isFinite(value) || value <= 0) {
    return "El precio debe ser mayor que 0.";
  }

  return undefined;
}

function validateZonaCapacidad(value: number) {
  if (!Number.isInteger(value) || value <= 0) {
    return "La capacidad debe ser un entero mayor que 0.";
  }

  return undefined;
}

function validateEvento(evento: Evento): EventoErrors {
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

function hasErrors(errors: EventoErrors) {
  return Boolean(
    errors.nombre ||
      errors.fecha ||
      errors.lugar ||
      errors.zonas.some(
        (zona) => zona.nombre || zona.precio || zona.capacidad,
      ),
  );
}

function parseNumberInput(value: string) {
  const parsed = Number(value);

  return Number.isFinite(parsed) ? parsed : 0;
}

function getInputClass(hasError: boolean, variant: "default" | "white" = "default") {
  const baseClass =
    "w-full rounded-2xl border px-4 py-3 text-sm text-zinc-950 outline-none transition focus:bg-white focus:ring-4";
  const surfaceClass =
    variant === "white" ? "bg-white" : "bg-zinc-50";
  const stateClass = hasError
    ? "border-rose-300 focus:border-rose-500 focus:ring-rose-100"
    : "border-zinc-200 focus:border-emerald-500 focus:ring-emerald-100";

  return `${baseClass} ${surfaceClass} ${stateClass}`;
}

function getErrorMessage(error?: string) {
  if (!error) {
    return null;
  }

  return <p className="text-sm text-rose-600">{error}</p>;
}

export default function EventoForm({
  title = "Crear Evento",
  description = "El formulario genera un objeto con nombre, fecha, lugar y un arreglo de zonas.",
  submitLabel = "Guardar evento",
}: EventoFormProps) {
  const [evento, setEvento] = useState<Evento>(initialEvento);
  const [errors, setErrors] = useState<EventoErrors>(
    getEmptyErrors(initialEvento.zonas.length),
  );
  const [formVersion, setFormVersion] = useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitState, setSubmitState] = useState<SubmitState>({
    status: "idle",
  });

  const updateField = <K extends keyof Omit<Evento, "zonas">>(
    field: K,
    value: Evento[K],
  ) => {
    setEvento((current) => ({
      ...current,
      [field]: value,
    }));
  };

  const updateFieldError = <K extends keyof Omit<EventoErrors, "zonas">>(
    field: K,
    value: EventoErrors[K],
  ) => {
    setErrors((current) => ({
      ...current,
      [field]: value,
    }));
  };

  const updateZona = <K extends keyof Zona>(
    index: number,
    field: K,
    value: Zona[K],
  ) => {
    setEvento((current) => ({
      ...current,
      zonas: current.zonas.map((zona, zonaIndex) =>
        zonaIndex === index ? { ...zona, [field]: value } : zona,
      ),
    }));
  };

  const updateZonaError = <K extends keyof ZonaErrors>(
    index: number,
    field: K,
    value: ZonaErrors[K],
  ) => {
    setErrors((current) => ({
      ...current,
      zonas: current.zonas.map((zona, zonaIndex) =>
        zonaIndex === index ? { ...zona, [field]: value } : zona,
      ),
    }));
  };

  const handleFieldBlur = (
    field: keyof Omit<Evento, "zonas">,
    rawValue: string,
  ) => {
    const value = field === "fecha" ? toJsonDate(rawValue) : rawValue.trim();

    updateField(field, value);

    const nextError =
      field === "nombre"
        ? validateNombre(value)
        : field === "fecha"
          ? validateFecha(value)
          : validateLugar(value);

    updateFieldError(field, nextError);
  };

  const handleZonaBlur = (
    index: number,
    field: keyof Zona,
    rawValue: string,
  ) => {
    if (field === "nombre") {
      const value = rawValue.trim();

      updateZona(index, field, value);
      updateZonaError(index, field, validateZonaNombre(value));
      return;
    }

    const parsedValue = parseNumberInput(rawValue);

    updateZona(index, field, parsedValue);
    updateZonaError(
      index,
      field,
      field === "precio"
        ? validateZonaPrecio(parsedValue)
        : validateZonaCapacidad(parsedValue),
    );
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const nextErrors = validateEvento(evento);
    setErrors(nextErrors);

    if (hasErrors(nextErrors)) {
      setSubmitState({
        status: "error",
        message: "Corrige los errores antes de enviar el formulario.",
      });
      return;
    }

    setIsSubmitting(true);
    setSubmitState({ status: "idle" });

    try {
      const response = await fetch("/api/eventos", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(evento),
      });

      const result = (await response.json()) as {
        ok?: boolean;
        message?: string;
      };

      if (!response.ok || !result.ok) {
        setSubmitState({
          status: "error",
          message: result.message ?? "No se pudo enviar el evento al API.",
        });
        return;
      }

      setSubmitState({
        status: "success",
        message: result.message ?? "Evento enviado correctamente al API.",
      });
    } catch {
      setSubmitState({
        status: "error",
        message: "Ocurrio un error de red al enviar el evento.",
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const addZona = () => {
    setEvento((current) => ({
      ...current,
      zonas: [
        ...current.zonas,
        {
          nombre: "",
          precio: 0,
          capacidad: 0,
        },
      ],
    }));
    setErrors((current) => ({
      ...current,
      zonas: [
        ...current.zonas,
        {
          nombre: undefined,
          precio: undefined,
          capacidad: undefined,
        },
      ],
    }));
  };

  const removeZona = (index: number) => {
    setEvento((current) => ({
      ...current,
      zonas: current.zonas.filter((_, zonaIndex) => zonaIndex !== index),
    }));
    setErrors((current) => ({
      ...current,
      zonas: current.zonas.filter((_, zonaIndex) => zonaIndex !== index),
    }));
  };

  const resetForm = () => {
    setEvento(initialEvento);
    setErrors(getEmptyErrors(initialEvento.zonas.length));
    setFormVersion((current) => current + 1);
    setSubmitState({ status: "idle" });
  };

  return (
    <section className="grid w-full max-w-6xl gap-6 lg:grid-cols-[minmax(0,1.2fr)_minmax(320px,0.8fr)]">
      <div className="rounded-3xl border border-black/5 bg-white p-8 shadow-[0_20px_80px_rgba(15,23,42,0.08)] sm:p-10">
        <div className="mb-8 space-y-3">
          <span className="inline-flex rounded-full bg-emerald-50 px-3 py-1 text-sm font-medium text-emerald-700">
            Formulario
          </span>
          <div className="space-y-2">
            <h1 className="text-3xl font-semibold tracking-tight text-zinc-950">
              {title}
            </h1>
            <p className="text-sm leading-6 text-zinc-600 sm:text-base">
              {description}
            </p>
          </div>
        </div>

        <form
          key={formVersion}
          className="space-y-8"
          onSubmit={handleSubmit}
          noValidate
        >
          <div className="grid gap-6 sm:grid-cols-2">
            <label className="space-y-2 text-sm font-medium text-zinc-800">
              <span>Nombre</span>
              <input
                type="text"
                name="nombre"
                defaultValue={evento.nombre}
                onBlur={(event) => handleFieldBlur("nombre", event.target.value)}
                className={getInputClass(Boolean(errors.nombre))}
              />
              {getErrorMessage(errors.nombre)}
            </label>

            <label className="space-y-2 text-sm font-medium text-zinc-800">
              <span>Fecha</span>
              <input
                type="date"
                name="fecha"
                defaultValue={toDateInput(evento.fecha)}
                onBlur={(event) => handleFieldBlur("fecha", event.target.value)}
                className={getInputClass(Boolean(errors.fecha))}
              />
              {getErrorMessage(errors.fecha)}
            </label>
          </div>

          <label className="space-y-2 text-sm font-medium text-zinc-800">
            <span>Lugar</span>
            <input
              type="text"
              name="lugar"
              defaultValue={evento.lugar}
              onBlur={(event) => handleFieldBlur("lugar", event.target.value)}
              className={getInputClass(Boolean(errors.lugar))}
            />
            {getErrorMessage(errors.lugar)}
          </label>

          <div className="space-y-4">
            <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
              <div>
                <h2 className="text-lg font-semibold text-zinc-950">Zonas</h2>
                <p className="text-sm text-zinc-500">
                  Cada zona debe incluir nombre, precio y capacidad.
                </p>
              </div>
              <button
                type="button"
                onClick={addZona}
                className="rounded-full border border-emerald-200 bg-emerald-50 px-4 py-2 text-sm font-medium text-emerald-700 transition hover:border-emerald-300 hover:bg-emerald-100"
              >
                Agregar zona
              </button>
            </div>

            <div className="space-y-4">
              {evento.zonas.map((zona, index) => (
                <div
                  key={index}
                  className="rounded-2xl border border-zinc-200 bg-zinc-50/80 p-4"
                >
                  <div className="mb-4 flex items-center justify-between gap-3">
                    <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-zinc-500">
                      Zona {index + 1}
                    </h3>
                    <button
                      type="button"
                      onClick={() => removeZona(index)}
                      disabled={evento.zonas.length === 1}
                      className="text-sm font-medium text-rose-600 transition hover:text-rose-700 disabled:cursor-not-allowed disabled:text-zinc-300"
                    >
                      Eliminar
                    </button>
                  </div>

                  <div className="grid gap-4 sm:grid-cols-[minmax(0,1fr)_140px_140px]">
                    <label className="space-y-2 text-sm font-medium text-zinc-800">
                      <span>Nombre</span>
                      <input
                        type="text"
                        defaultValue={zona.nombre}
                        onBlur={(event) =>
                          handleZonaBlur(index, "nombre", event.target.value)
                        }
                        className={getInputClass(
                          Boolean(errors.zonas[index]?.nombre),
                          "white",
                        )}
                      />
                      {getErrorMessage(errors.zonas[index]?.nombre)}
                    </label>

                    <label className="space-y-2 text-sm font-medium text-zinc-800">
                      <span>Precio</span>
                      <input
                        type="number"
                        min="0"
                        step="0.01"
                        defaultValue={zona.precio}
                        onBlur={(event) =>
                          handleZonaBlur(index, "precio", event.target.value)
                        }
                        className={getInputClass(
                          Boolean(errors.zonas[index]?.precio),
                          "white",
                        )}
                      />
                      {getErrorMessage(errors.zonas[index]?.precio)}
                    </label>

                    <label className="space-y-2 text-sm font-medium text-zinc-800">
                      <span>Capacidad</span>
                      <input
                        type="number"
                        min="0"
                        step="1"
                        defaultValue={zona.capacidad}
                        onBlur={(event) =>
                          handleZonaBlur(index, "capacidad", event.target.value)
                        }
                        className={getInputClass(
                          Boolean(errors.zonas[index]?.capacidad),
                          "white",
                        )}
                      />
                      {getErrorMessage(errors.zonas[index]?.capacidad)}
                    </label>
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className="flex flex-col gap-3 border-t border-zinc-100 pt-6 sm:flex-row sm:items-center sm:justify-between">
            <div className="space-y-1">
              <p className="text-sm text-zinc-500">
                La vista previa refleja exactamente la estructura JSON solicitada.
              </p>
              {submitState.status !== "idle" ? (
                <p
                  className={`text-sm ${
                    submitState.status === "success"
                      ? "text-emerald-600"
                      : "text-rose-600"
                  }`}
                >
                  {submitState.message}
                </p>
              ) : null}
            </div>
            <div className="flex gap-3">
              <button
                type="button"
                onClick={resetForm}
                className="rounded-full border border-zinc-200 px-5 py-2.5 text-sm font-medium text-zinc-700 transition hover:border-zinc-300 hover:bg-zinc-50"
                disabled={isSubmitting}
              >
                Restablecer
              </button>
              <button
                type="submit"
                disabled={isSubmitting}
                className="rounded-full bg-zinc-950 px-5 py-2.5 text-sm font-medium text-white transition hover:bg-zinc-800 disabled:cursor-not-allowed disabled:bg-zinc-400"
              >
                {isSubmitting ? "Enviando..." : submitLabel}
              </button>
            </div>
          </div>
        </form>
      </div>

      <aside className="rounded-3xl border border-zinc-200 bg-zinc-950 p-6 text-zinc-50 shadow-[0_20px_80px_rgba(15,23,42,0.12)] sm:p-8">
        <div className="mb-4 space-y-2">
          <p className="text-xs font-semibold uppercase tracking-[0.3em] text-emerald-300">
            JSON resultante
          </p>
          <h2 className="text-2xl font-semibold">Payload del evento</h2>
          <p className="text-sm leading-6 text-zinc-400">
            Este objeto ya está listo para enviarse a una API o server action.
          </p>
        </div>

        <pre className="overflow-x-auto rounded-2xl bg-white/5 p-4 text-xs leading-6 text-emerald-100 sm:text-sm">
          <code>{JSON.stringify(evento, null, 2)}</code>
        </pre>
      </aside>
    </section>
  );
}