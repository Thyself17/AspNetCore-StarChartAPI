﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var obj = _context.CelestialObjects.Find(id);
            if (obj == null)
                return NotFound();

            obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var objs = _context.CelestialObjects.Where(o=>o.Name == name).ToList();
            if (!objs.Any())
                return NotFound();

            foreach (var celestialObject in objs)
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
            return Ok(objs);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var objs = _context.CelestialObjects.ToList();
            foreach (var obj in objs)
            {
                obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(objs);
        }
    }
}
