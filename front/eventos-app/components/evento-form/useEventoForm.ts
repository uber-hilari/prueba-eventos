import { useState } from "react";
import type { Evento, EventoErrors, SubmitState, Zona, ZonaErrors } from "./types";
import {
  createEmptyZona,
  createEmptyZonaErrors,
  getEmptyErrors,
  hasErrors,
  initialEvento,
  parseNumberInput,
  toJsonDate,
  validateEvento,
  validateFecha,
  validateLugar,
  validateNombre,
  validateZonaCapacidad,
  validateZonaNombre,
  validateZonaPrecio,
} from "./utils";

export default function useEventoForm() {
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
      zonas: [...current.zonas, createEmptyZona()],
    }));
    setErrors((current) => ({
      ...current,
      zonas: [...current.zonas, createEmptyZonaErrors()],
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

  return {
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
  };
}
