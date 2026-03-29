"use client";

import EventoFields from "./evento-form/EventoFields";
import ZonaList from "./evento-form/ZonaList";
import type { EventoFormProps } from "./evento-form/types";
import useEventoForm from "./evento-form/useEventoForm";

export default function EventoForm({
  title = "Crear Evento",
  description = "El formulario genera un objeto con nombre, fecha, lugar y un arreglo de zonas.",
  submitLabel = "Guardar evento",
}: EventoFormProps) {
  const {
    evento,
    errors,
    formVersion,
    isSubmitting,
    submitState,
    addZona,
    handleFieldBlur,
    handleSubmit,
    handleZonaBlur,
    removeZona,
    resetForm,
  } = useEventoForm();

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
          <EventoFields
            evento={evento}
            errors={errors}
            onFieldBlur={handleFieldBlur}
          />

          <ZonaList
            zonas={evento.zonas}
            errors={errors.zonas}
            onAddZona={addZona}
            onRemoveZona={removeZona}
            onZonaBlur={handleZonaBlur}
          />

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
